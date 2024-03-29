﻿using Microsoft.Playwright;
using Xunit;

[CollectionDefinition(nameof(PlaywrightFixture))]
public class SharedPlaywrightCollection : ICollectionFixture<PlaywrightFixture> {}

// ReSharper disable once ClassNeverInstantiated.Global
public class PlaywrightFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        PlaywrightInstance = await Playwright.CreateAsync();
        Browser = await PlaywrightInstance.Chromium.LaunchAsync();
    }

    public IBrowser Browser { get; set; } = null!;
    private IPlaywright PlaywrightInstance { get; set; } = null!;

    public async Task DisposeAsync()
    {
        await Browser.DisposeAsync();
        PlaywrightInstance.Dispose();
    }
}