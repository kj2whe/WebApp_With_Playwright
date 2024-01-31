using Microsoft.Playwright;
using Xunit;
using System.Diagnostics;

namespace WebTests;

public class UnitTest1 : IClassFixture<PlaywrightFixture>, IDisposable
{
    private IBrowser Browser { get; }
    private Process _serverProcess;
    
    public UnitTest1(PlaywrightFixture fixture)
    {
        Browser = fixture.Browser;
        StartServer();
    }
    
    private void StartServer()
    {
        // Command to start your server (adjust as necessary)
        string serverStartCommand = "dotnet run";
        
        // Configure and start the server process
        _serverProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {serverStartCommand}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = @"C:/Users/jason/RiderProjects/webapp_with_playwright" // Set your project directory
            }
        };
        _serverProcess.Start();
    }
    
    public void Dispose()
    {
        // Stop the server when the test is done
        if (!_serverProcess.HasExited)
        {
            _serverProcess.Kill();
        }
    }
   
  
    [Fact]
    public async Task TestHeaderTextAndLink()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false // Set to false to see the browser
        });
        var page = await browser.NewPageAsync();

        // Navigate to your page containing the HTML
        await page.GotoAsync("http://localhost:5006");

        // Assert that the H1 text is "Welcome"
        var headerText = await page.InnerTextAsync(".display-4"); // Using the class of the h1 tag
        Assert.Equal("Welcome", headerText);

        // Get the href attribute of the link
        var linkHref = await page.GetAttributeAsync("a[href='https://learn.microsoft.com/aspnet/core']", "href");

        // Assert the link navigates to the correct URL
        Assert.Equal("https://learn.microsoft.com/aspnet/core", linkHref);

        // Optionally, you can click the link and assert the navigation
        // Uncomment the following lines if needed
        // await page.ClickAsync("a[href='https://learn.microsoft.com/aspnet/core']");
        // Assert.Equal("https://learn.microsoft.com/aspnet/core", page.Url);
    }
    
    // [Fact]
    // public async Task TestDivColorChange()
    // {
    //     using var playwright = await Playwright.CreateAsync();
    //     await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
    //     {
    //         Headless = true // Set to false if you want to see the browser UI
    //     });
    //     var page = await browser.NewPageAsync();
    //
    //     // Navigate to your page
    //     await page.GotoAsync("http://localhost:5006");
    //
    //     // Click the button
    //     await page.ClickAsync("#yourButtonId"); // Replace with your button's ID or selector
    //
    //     // Wait for the color change to take effect
    //     await Task.Delay(1000); // Adjust the delay as needed
    //
    //     // Get the background color of the div
    //     var color = await page.EvaluateAsync<string>("() => getComputedStyle(document.querySelector('#yourDivId')).backgroundColor"); // Replace with your div's ID or selector
    //
    //     // Assert that the color is blue
    //     Assert.Equal("rgb(0, 0, 255)", color); // Adjust the expected color value as necessary
    //
    //     // The browser will automatically close when the using block ends
    // }
    
}