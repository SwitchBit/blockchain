using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace SwitchBit.Blockchain.Rpc
{
    /// <summary>
    /// Utility class to communicate with a bitcoin Remote Procedure Call server to query statistics, block and transaction information
    /// as well as mempool and account information
    /// </summary>
    public class BitcoinRpcClient : BlockchainRpcClient
    {
        /// <summary>
        /// Creates a new instance of a BitcoinRpcClient. This may be used to query general information about the bitcoin process 
        /// running on the remote machine. Transactions and such may come in a future version. 
        /// </summary>
        /// <param name="rpcUsername">the username that is set in the server's bitcoin.conf file</param>
        /// <param name="rpcPassword">the password that is set in the server's bitcoin.conf file</param>
        /// <param name="rpcUrl">the URL to the RPC server, default is 'http://localhost:8332'</param>
        public BitcoinRpcClient(string rpcUsername, string rpcPassword, string rpcUrl = "http://localhost:8332") 
            : base(rpcUsername, rpcPassword, rpcUrl)
        {
            //just pass everything to the base
        }

        /// <summary>
        /// Returns the hash of a block, given it's block number / block height
        /// </summary>
        /// <param name="blockNumber">integer block number or specific block height</param>
        /// <returns>string hash representation of the corresponding block</returns>
        public async Task<string> GetBlockHash(int blockNumber = 0)
        {
            var hash = await InvokeMethod("getblockhash", blockNumber);
            return hash.GetValue("result").ToString();
        }

        //TODO: Update docs for GetBestBlockHash()

        /// <summary>
        /// Returns the best block hash (Don't know what this does differently yet)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBestBlockHash()
        {
            var hash = await InvokeMethod("getbestblockhash");
            return hash.ToString();
        }

        /// <summary>
        /// Retrieves a BitcoinBlock object populated from the bitcoin daemon
        /// </summary>
        /// <param name="blockHash">the hash of the block, can be obtained from 'getblockhash'</param>
        /// <returns>a populated BitcoinBlock</returns>
        public async Task<BitcoinBlock> GetBlock(string blockHash)
        {
            var block = await InvokeMethod("getblock", blockHash);
            var retVal = block.GetValue("result").ToObject<BitcoinBlock>();
            retVal.Height = int.Parse(JObject.Parse(block.GetValue("result").ToString()).GetValue("height").ToString());
            return retVal;
        }

        /// <summary>
        /// Gets a json blob of information about the network
        /// </summary>
        /// <returns>json blob</returns>
        public async Task<JObject> GetInfo()
        {
            var info = await InvokeMethod("getinfo");
            return info;
        }

        /// <summary>
        /// Retrieves a json blob with the information about a specific transaction
        /// ***Only works on transactions local to the daemon's wallet, use 'getrawtransaction' if arbitrary tx info is needed
        /// </summary>
        /// <param name="transactionId">transactionid from the list of ids stored in a block record</param>
        /// <returns>json blob with the transaction</returns>
        public async Task<JObject> GetTransaction(string transactionId)
        {
            var tx = await InvokeMethod("gettransaction", transactionId);
            return tx;
        }

        //NOTES ON TRANSACTION INDEXING
        //To make getrawtransaction work, you have to reindex all the blocks with their transactions, -txindex=1 -reindex=1    
        //"D:\Bitcoin Node\bitcoin-qt.exe" -rpcthreads=16 -rpcallowip=192.168.0.0/24 -rpcuser=test -rpcpassword=1qaz@WSX -txindex=1 -reindex=1 -server -datadir=D:\BlockChains\BTC-MAIN
        //Then when it's complete, remove the -reindex=1 but leave the -txindex=1
        //"D:\Bitcoin Node\bitcoin-qt.exe" -rpcthreads=16 -rpcallowip=192.168.0.0/24 -rpcuser=test -rpcpassword=1qaz@WSX -txindex=1 -reindex=1 -server -datadir=D:\BlockChains\BTC-MAIN

        /// <summary>
        /// Retrieves an arbitrary BitcoinTransaction object representing the details of a given transaction
        /// ***
        /// Requires 
        ///     'bitcoin-qt.exe -txindex=1 -reindex=1 -server
        /// Then once finished
        ///     'bitcoin-qt.exe -txindex=1 -server'
        /// ***
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public async Task<BitcoinTransaction> GetRawTransaction(string transactionId, string verbose = "1")
        {
            var obj = await InvokeMethod("getrawtransaction", transactionId, 1); //Invalid type provided. Verbose parameter must be a boolean. - error code -3 if not, "1" works, true doesnt....
            var tx = obj.GetValue("result").ToObject<BitcoinTransaction>();
            return tx;
        }

        /// <summary>
        /// Returns the total number of blocks found to date on the bitcoin network
        /// </summary>
        /// <returns>the number of blocks found so far</returns>
        public async Task<int> GetBlockCount()
        {
            var retVal = await InvokeMethod("getblockcount");
            return retVal.GetValue("result").ToObject<int>();
        }

        /// <summary>
        /// Returns the current difficulty value of the bitcoin network
        /// </summary>
        /// <returns>The number value of the current difficulty</returns>
        public async Task<double> GetDifficulty()
        {
            var retVal = await InvokeMethod("getdifficulty");
            return retVal.GetValue("result").ToObject<double>();
        }

        /// <summary>
        /// Gets the raw mempool statistics for temporary transaction storage
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> GetRawMempool()
        {
            var mempoolInfo = await InvokeMethod("getrawmempool");
            return mempoolInfo;
        }

        /// <summary>
        /// Gets a BitcoinAccount object
        /// </summary>
        /// <returns>a populated BitcoinAccount object</returns>
        public async Task<BitcoinAccount> GetAccount()
        {
            var retVal = await InvokeMethod("getaccount");
            Console.WriteLine($@"Account {retVal}");
            return retVal.GetValue("result").ToObject<BitcoinAccount>();
        }
    }
}
