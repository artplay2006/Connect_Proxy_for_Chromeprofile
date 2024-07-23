using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Net;

// Set the proxy configuration
int port = 0;
string name = "";
string password = "";
string host = "";
ChromeOptions options = new ChromeOptions();
options.AddArgument($"--proxy-server=http://{host}:{port}");

// Create a network authentication handler for the proxy
NetworkAuthenticationHandler handler = new NetworkAuthenticationHandler()
{
    UriMatcher = d => true,
    Credentials = new PasswordCredentials(name, password)
};

// Create the proxy URL
string proxy = $"http://{name}:{password}@{host}:{port}";

// Print the proxy being tested
Console.WriteLine("Currently testing: " + proxy);

// Start the ChromeDriver with the proxy configuration
using (IWebDriver driver = new ChromeDriver(options))
{

    //proxy authorization code
    var networkInterceptor = driver.Manage().Network;
    networkInterceptor.AddAuthenticationHandler(handler);
    networkInterceptor.StartMonitoring();
    networkInterceptor.StopMonitoring();

    try
    {
        // Navigate to the target URL
        driver.Navigate().GoToUrl("https://2ip.ru/");
        Thread.Sleep(3000);

        try
        {
            // Check if the proxy is working by looking for an element with the class "neterror"
            driver.FindElement(By.ClassName("neterror"));
            Console.Write("Proxy is not working: ");
        }
        catch
        {
            Console.Write("Proxy is working: ");
        }

        // Print the proxy being tested
        Console.WriteLine(proxy);
    }
    catch (Exception e)
    {
        // Handle any errors that occur during the process
        Console.WriteLine($"Error: {e}");
        Thread.Sleep(3000);
    }

    driver.Quit();
}