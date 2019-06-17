# `FastestBinaryStream`
Fluent interface for binary protocol implementation, aiming to prioritize runtime performance above all else.  An
alternative to `System.IO.MemoryStream` + `System.IO.BinaryReader` & `System.IO.BinaryWriter`, and also
`System.Convert`, that is better suited than the .NET (Core) implementations for performance-sensitive code (code for
low-power platforms, high-volume/low-latency systems, games, etc.).

Accepting [PRs](https://github.com/M-Pixel/FastestBinaryStream/pulls), [Wiki](https://github.com/M-Pixel/FastestBinaryStream/wiki) contributions, and [Issues](https://github.com/M-Pixel/FastestBinaryStream/issues) of all kinds: bugs, questions, complaints & suggestions, requests, etc.

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
