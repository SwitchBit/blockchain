using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Collections.Generic;

namespace SwitchBit.Blockchain.Rpc
{
    public abstract class BlockchainRpcClient : IBlockchainRpcClient
    {
        /// <summary>
        /// Creates an instance of the BlockchainRpcClient class
        /// </summary>
        /// <param name="rpcUsername"></param>
        /// <param name="rpcPassword"></param>
        /// <param name="rpcUrl"></param>
        public BlockchainRpcClient(string rpcUsername, string rpcPassword, string rpcUrl = "http://localhost:8332")
        {
            RpcUsername = rpcUsername;
            RpcPassword = rpcPassword;
            RpcUrl = rpcUrl;
        }
        
        //https://en.bitcoin.it/wiki/Running_Bitcoin
        
        /// <summary>
        /// Gets or Sets the Url for the RPC server
        /// <code>http://127.0.0.1:8332</code>
        /// </summary>
        public string RpcUrl { get; private set; }
        /// <summary>
        /// Gets or sets the Rpc password to use during remote calls
        /// </summary>
        public string RpcPassword { get; private set; }
        /// <summary>
        /// Gets or sets the Rpc username to use during remote calls
        /// </summary>
        public string RpcUsername { get; private set; }

        /// <summary>
        /// Invoke a method on the RPC server
        /// </summary>
        /// <param name="methodName">the name of the method to invoke</param>
        /// <param name="parameters">parameters to the method being invoked</param>
        /// <returns>JObject of the result</returns>
        public async Task<JObject> InvokeMethod(string methodName, params object[] parameters)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(RpcUrl);

            if (!string.IsNullOrEmpty(RpcUsername) && !string.IsNullOrEmpty(RpcPassword))
                webRequest.Credentials = new NetworkCredential(RpcUsername, RpcPassword);

            webRequest.ContentType = "application/json-rpc";
            webRequest.Method = "POST";
            
            var postObject = new JObject
            {
                ["jsonrpc"] = "1.0",
                ["id"] = "1",
                ["method"] = methodName
            };

            postObject.Add(new JProperty("params", new List<object>(parameters).ToArray()));

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postObject));

            //webRequest.ContentLength = bytes.Length;

            using (var requestStream = await webRequest.GetRequestStreamAsync())
                await requestStream.WriteAsync(bytes, 0, bytes.Length);
                
            try
            {
                using (var webResponse = await webRequest.GetResponseAsync())
                using (var responseStream = webResponse.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                    return JsonConvert.DeserializeObject<JObject>(await reader.ReadToEndAsync());
                 
            }
            catch (WebException webex)
            {
                using (var responseStream = webex.Response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    var returnJson = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());
                    Debug.WriteLine($@"[Blockchain.Rpc.BlockchainRpcClient.InvokeMethod] ERROR: {methodName}, {postObject.GetValue("params")}), Response: {returnJson}");
                    return returnJson;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($@"[Blockchain.Rpc.BlockchainRpcClient.InvokeMethod] ERROR: {exception.ToString()}");
                throw;
            }
        }
    }
}
