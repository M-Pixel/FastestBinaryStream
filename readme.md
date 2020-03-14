# `FastestBinaryStream`
Fluent interface for binary protocol implementation, aiming to prioritize runtime performance above all else.  An
alternative to `System.IO.MemoryStream` + `System.IO.BinaryReader` & `System.IO.BinaryWriter`, and also
`System.Convert`, that is better suited than the .NET (Core) implementations for performance-sensitive code (code for
low-power platforms, high-volume/low-latency systems, games, etc.).

Accepting [PRs](https://github.com/M-Pixel/FastestBinaryStream/pulls), [Wiki](https://github.com/M-Pixel/FastestBinaryStream/wiki) contributions, and [Issues](https://github.com/M-Pixel/FastestBinaryStream/issues) of all kinds: bugs, questions, complaints & suggestions, requests, etc.

## Example
Here's an example that converts a public key from its raw form into an SSH-formatted public key string:

```csharp
string FormatForSSH(string publicKeyAlgorithm, byte[] publicKeyData) =>
    new BinaryStream(sizeof(int) + publicKeyAlgorithm.Length + sizeof(int) + publicKeyData.Length)
        .WriteBig(KeyType.Length) // write an integer in big-endian byte order
        .Write(Encoding.ASCII.GetBytes(publicKeyAlgorithm)) // Convert the string from UTF-16 to ASCII and write it
        .WriteBig(publicKeyData.Length) // Note that each Write method advances the stream's head, concatenating values
        .Write(publicKey) // write an array of bytes, verbatim
        .FlushBase64String(out var sshPublicKey) // Dump the entire stream (automatically rewinding the head)
        .DisposeAndReturn(sshPublicKey); // Dispose the allocated buffer, and return the final value
```

## To-Do
- Test, and evaluate IL, to ensure that implementations are, in fact, the fastest possible
- Implement remaining overload permutations of existing methods
- Unit tests
- Implement remaining Stream, BinaryReader, and BinaryWriter methods
- Non-ref struct, and methods to convert to/from it
- Conditionally compiled safety checks
- Investigate ways to reduce repetition without impacting resulting IL
- Codegen for formulaic/repetitive methods
- Logo/icon
