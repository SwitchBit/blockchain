# Blockchain Utilities

This is a pretty small library to query a blockchain's RPC server for information about the network or transactions.

The toolset is written in C#, using .Net Standard 1.3

# Caveat(s)

In order for 'getrawtransaction' to work properly for arbitrary transaction lookups, you'll need to re-index the blockchain on disk to include all of the transaction information as well. 

Notes are in the project