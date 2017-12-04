using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SwitchBit.Blockchain.Rpc
{
    public class BitcoinTransaction
    {
        [Key]
        [JsonProperty("txid")]
        public string TransactionId { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("vsize")]
        public int VSize { get; set; }
        [JsonProperty("locktime")]
        public int LockTime { get; set; }
        [JsonProperty("vin")]
        public List<BitcoinTransactionInput> BitcoinTransactionInputs { get; set; }
        [JsonProperty("vout")]
        public List<BitcoinTransactionOutput> BitcoinTransactionOutputs { get; set; }
        [JsonProperty("hex")]
        public string Hex { get; set; }
        [JsonProperty("blockhash")]
        public string BlockHash { get; set; } //TODO: Index for this 
        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("blocktime")]
        public int Blocktime { get; set; }
    }
    
    public class BitcoinTransactionInput
    {
        [Key]
        public Guid InputId { get; set; }
        [JsonProperty("coinbase")]
        public string Coinbase { get; set; }
        [JsonProperty("sequence")]
        public long Sequence { get; set; }
    }

    public class BitcoinTransactionOutput
    {
        [Key]
        public Guid OutputId { get; set; }
        [JsonProperty("scriptPubKey")]
        public ScriptPubKey ScriptPublicKey { get; set; }
        [JsonProperty("value")]
        public float Value { get; set; }
        [JsonProperty("n")]
        public int N { get; set; }
    }

    public class ScriptPubKey
    {
        [Key]
        public Guid PubKeyScriptId { get; set; }
        [JsonProperty("hex")]
        public string Hex { get; set; }
        [JsonProperty("asm")]
        public string Asm { get; set; }
        [JsonProperty("reqSigs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RequiredSignatures { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("addresses")]
        public string[] Addresses { get; set; }
    }
}
