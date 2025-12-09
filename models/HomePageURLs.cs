using System;
using System.IO;
using System.Text.Json;
using FirstPlaywrightTest.utils;

namespace FirstPlaywrightTest.models
{
    public class HomePageURLs
    {
        public HomepageEnvironmentURLs homepage { get; private set; } = new HomepageEnvironmentURLs();
        public string Url { get; private set; } = string.Empty;

        public class HomepageEnvironmentURLs
        {
            public string Dev { get; set; } = string.Empty;
            public string Qa { get; set; } = string.Empty;
            public string Uat { get; set; } = string.Empty;
        }

        // Instance constructor: loads JSON and selects the URL for the provided env.
        public HomePageURLs(string env, string? filePath = null)
        {
            filePath = ResolveFilePath(filePath);
            var wrapper = JsonDataLoader.Load<Wrapper>(filePath)
                          ?? throw new InvalidOperationException("Failed to deserialize Homepage.json");

            homepage = wrapper.homepage ?? throw new InvalidOperationException("Missing 'homepage' element in Homepage.json");

            Url = PickUrl(env);
        }

        // One-line convenience: load and return the selected URL (preferred for simple calls).
        public static string GetUrl(string env, string? filePath = null)
        {
            filePath = ResolveFilePath(filePath);
            var wrapper = JsonDataLoader.Load<Wrapper>(filePath)
                          ?? throw new InvalidOperationException("Failed to deserialize Homepage.json");

            var home = wrapper.homepage ?? throw new InvalidOperationException("Missing 'homepage' element in Homepage.json");

            return env.ToLower() switch
            {
                "qa" => home.Qa,
                "dev" => home.Dev,
                "uat" => home.Uat,
                _ => throw new ArgumentException($"No such environment as: {env}", nameof(env))
            };
        }

        // Optional convenience getter for instances
        public string GetURL() => Url;

        // helper to pick url from populated `homepage`
        private string PickUrl(string env) => env.ToLower() switch
        {
            "qa" => homepage.Qa,
            "dev" => homepage.Dev,
            "uat" => homepage.Uat,
            _ => throw new ArgumentException($"No such environment as: {env}", nameof(env))
        };

        // Resolve stable file path (test output-friendly)
        private static string ResolveFilePath(string? filePath)
        {
            filePath ??= Path.Combine(AppContext.BaseDirectory, "testData", "homepage", "Homepage.json");

            if (!File.Exists(filePath))
            {
                // fallback for different working dir layouts
                filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "testData", "homepage", "Homepage.json");
            }

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Homepage.json not found", filePath);

            return filePath;
        }

        // private helper wrapper to avoid deserializing into this exact type (safe for constructor)
        private class Wrapper
        {
            public HomepageEnvironmentURLs homepage { get; set; } = new HomepageEnvironmentURLs();
        }
    }
}