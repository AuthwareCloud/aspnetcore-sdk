using Authware.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Authware.Tests;

public class IntegrationTests
{
    private const string AppId = "";
    private const string Key = "";

    private IServiceProvider _serviceProvider;
    
    [SetUp]
    public void Setup()
    {
        var collection = new ServiceCollection();
        collection.AddHttpClient();
        collection.AddAuthware(options =>
        {
            options.AppId = AppId;
        });
        
        _serviceProvider = collection.BuildServiceProvider();
    }

    [Test]
    public async Task GetProfileWithApiKey_NoException()
    {
        var authware = _serviceProvider.GetRequiredService<AuthwareApplication>();

        var profile = await authware.GetUserProfileAsync(Key, true);
        Assert.That(profile.Username, Is.Not.Null);
    }
}