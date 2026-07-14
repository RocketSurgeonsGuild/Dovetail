using Rocket.Surgery.Extensions.Testing;


// ReSharper disable ObjectCreationAsStatement
#pragma warning disable CA1806

namespace Dovetail.Tests;

public class DovetailTests() : AutoFakeTest<TestRecord>(TestRecord.Create()) { }
