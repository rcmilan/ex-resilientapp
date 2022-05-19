using System.Text.Json;

namespace App.Helpers
{
    public static class Extensions
    {
        public static T DeserializeContent<T>(this HttpResponseMessage source)
        {
            var resultString = source.Content
                .ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(resultString))
                return default!;

            return JsonSerializer.Deserialize<T>(resultString);
        }
    }
}
