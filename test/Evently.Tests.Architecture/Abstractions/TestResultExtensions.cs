using AwesomeAssertions;
using NetArchTest.Rules;

namespace Evently.Tests.Architecture.Abstractions;

internal static class TestResultExtensions
{
    internal static void ShouldBeSuccessful(this TestResult testResult)
    {
        testResult.FailingTypes?.Should().BeEmpty();
    }
}
