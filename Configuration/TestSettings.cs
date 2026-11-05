namespace bai_thu.Configuration;

public sealed class TestSettings
{
    public string BaseUrl { get; }
    public string Browser { get; }
    public bool Headless { get; }
    public int WaitSeconds { get; }

    private TestSettings(string baseUrl, string browser, bool headless, int waitSeconds)
    {
        BaseUrl = baseUrl;
        Browser = browser;
        Headless = headless;
        WaitSeconds = waitSeconds;
    }

    public static TestSettings Load()
    {
        var baseUrl = GetSetting("BASE_URL", "https://the-internet.herokuapp.com");
        var browser = GetSetting("BROWSER", "chrome").ToLowerInvariant();
        var headlessValue = Environment.GetEnvironmentVariable("HEADLESS");
        var headless = string.IsNullOrWhiteSpace(headlessValue)
            ? false
            : bool.TryParse(headlessValue, out var parsedHeadless)
                ? parsedHeadless
                : throw new FormatException("HEADLESS must be true or false.");

        var waitValue = Environment.GetEnvironmentVariable("WAIT_SECONDS");
        var waitSeconds = string.IsNullOrWhiteSpace(waitValue)
            ? 10
            : int.TryParse(waitValue, out var parsedWait)
                ? parsedWait
                : throw new FormatException("WAIT_SECONDS must be a positive integer.");

        if (waitSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(waitSeconds), "WAIT_SECONDS must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(baseUrl)
            || !Uri.TryCreate(baseUrl, UriKind.Absolute, out var parsedBaseUrl)
            || parsedBaseUrl.Scheme is not ("http" or "https"))
        {
            throw new FormatException("BASE_URL must be an absolute HTTP or HTTPS URL.");
        }

        if (browser is not ("chrome" or "firefox" or "edge"))
        {
            throw new FormatException("BROWSER must be one of: chrome, firefox, edge.");
        }

        return new TestSettings(baseUrl.TrimEnd('/'), browser, headless, waitSeconds);
    }

    private static string GetSetting(string name, string defaultValue) =>
        Environment.GetEnvironmentVariable(name) is { Length: > 0 } value ? value : defaultValue;
}
