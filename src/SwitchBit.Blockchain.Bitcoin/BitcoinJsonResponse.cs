using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchBit.Blockchain.Rpc
{
    public class BitcoinJsonResponse<T>
    {
        public T result { get; set; }
        public object error { get; set; }
        public string id { get; set; }
    }
}
