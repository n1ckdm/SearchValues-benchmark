using System.Buffers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis;

BenchmarkRunner.Run<SearchBenchmark>();

[MemoryDiagnoser(false)]
[SimpleJob(RunStrategy.Throughput, iterationCount: 5)]
public class SearchBenchmark
{
    private static readonly string base64Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    private static readonly SearchValues<char> Base64SearchValues = SearchValues.Create(base64Characters);

    private static readonly HashSet<char> Base64HashSet = new HashSet<char>(base64Characters);

    [Params("asdioeoecnEFeciecqpoEJ", "asdioeoecnEF^eciecqpoEJ")]
    public string ExampleText { get; set; }

    [Benchmark]
    public bool IsBase64_WithChars()
    {
        return ExampleText.All(base64Characters.Contains);
    }

    [Benchmark]
    public bool IsBase64_WithHashSet()
    {
        return ExampleText.All(Base64HashSet.Contains);
    }

    [Benchmark]
    public bool IsBase64_WithSpan()
    {
        return ExampleText.AsSpan().IndexOfAnyExcept(base64Characters) == -1;
    }

    [Benchmark]
    public bool IsBase64_WithSeachValues()
    {
        return ExampleText.AsSpan().IndexOfAnyExcept(Base64SearchValues) == -1;
    }

}

