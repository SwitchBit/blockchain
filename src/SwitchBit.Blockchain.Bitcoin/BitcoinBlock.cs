using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SwitchBit.Blockchain.Rpc
{
    public class BitcoinBlockResponse
    {
        public BitcoinBlock result { get; set; }
        public object error { get; set; }
        public string id { get; set; }
    }

    public class BitcoinBlock
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("confirmations")]
        public double Confirmations { get; set; }
        [JsonProperty("strippedsize")]
        public double Strippedsize { get; set; }
        [JsonProperty("size")]
        public double Size { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("height ")]
        public int Height { get; set; }
        [JsonProperty("version")]
        public double Version { get; set; }
        [JsonProperty("versionHex")]
        public string VersionHex { get; set; }
        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; }
        [JsonProperty("tx")]
        [NotMapped()]
        public List<string> TransactionIds { get; set; }
        [JsonProperty("time")]
        public double Time { get; set; }
        [JsonProperty("mediantime")]
        public double MedianTime { get; set; }
        [JsonProperty("nonce")]
        public double Nonce { get; set; }
        [JsonProperty("bits")]
        public string Bits { get; set; }
        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }
        [JsonProperty("chainwork")]
        public string Chainwork { get; set; }
        [JsonProperty("nextblockhash")]
        public string NextBlockHash { get; set; }
    }
}
