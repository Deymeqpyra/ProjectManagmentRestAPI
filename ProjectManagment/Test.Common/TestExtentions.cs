using Newtonsoft.Json;

namespace Test.Common;

public static class TestExtentions
{
    public static async Task<T> ToResponeModel<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content)
            ?? throw new ArgumentException("Response content is null");
    }
}