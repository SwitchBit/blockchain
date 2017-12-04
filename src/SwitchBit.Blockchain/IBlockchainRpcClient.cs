using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace SwitchBit.Blockchain.Rpc
{
    public interface IBlockchainRpcClient
    {
        Task<JObject> InvokeMethod(string methodName, params object[] parameters);
    }
}