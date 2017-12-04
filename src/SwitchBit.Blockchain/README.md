# SwitchBit.Blockchain

Simple base class with support for calling JSON RPC methods hosted by a cryptocurrency's wallet or daemon service. 

## Things to keep in mind

* Make sure your `rpcusername` and `rpcpassword` are configured either via command line or the `bitcoin.conf` file
* Enable servicing RPC with `-server` on the command line or `server=1` within the `bitcoin.conf` file

## TODO txindex reindex howto