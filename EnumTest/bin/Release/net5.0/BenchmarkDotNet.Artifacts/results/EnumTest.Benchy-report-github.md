``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1237 (21H2)
Intel Core i7-7500U CPU 2.70GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=5.0.404
  [Host]     : .NET 5.0.13 (5.0.1321.56516), X64 RyuJIT
  DefaultJob : .NET 5.0.13 (5.0.1321.56516), X64 RyuJIT


```
|             Method |       Mean |     Error |    StdDev |     Median |  Gen 0 | Allocated |
|------------------- |-----------:|----------:|----------:|-----------:|-------:|----------:|
|     NativeToString | 40.9984 ns | 1.7339 ns | 4.9189 ns | 38.9303 ns | 0.0114 |      24 B |
|       FastToString |  1.2597 ns | 0.1162 ns | 0.1243 ns |  1.2620 ns |      - |         - |
| StringEnumToString |  0.1808 ns | 0.0851 ns | 0.1165 ns |  0.1556 ns |      - |         - |
|          SmartEnum |  0.0145 ns | 0.0184 ns | 0.0226 ns |  0.0000 ns |      - |         - |
