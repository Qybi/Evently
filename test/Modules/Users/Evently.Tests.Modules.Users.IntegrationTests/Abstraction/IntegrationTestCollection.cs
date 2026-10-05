namespace Evently.Tests.Modules.Users.IntegrationTests.Abstraction;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>;
