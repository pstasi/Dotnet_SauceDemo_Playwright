using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


//TODO: Update this util to declutter the poms and tests
namespace FirstPlaywrightTest.utils
{
    public class JsonDataLoader
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static T Load<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json, _options);
        }

    }
}
