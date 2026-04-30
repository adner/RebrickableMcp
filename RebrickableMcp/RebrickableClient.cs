using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace RebrickableMcp;

public sealed class RebrickableClient
{
    private readonly HttpClient _http;

    public RebrickableClient(HttpClient http, IOptions<RebrickableOptions> options)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://rebrickable.com/api/v3/");
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("key", options.Value.ApiKey);
    }

    public async Task<LegoSet?> GetSetAsync(string setNumber, CancellationToken cancellationToken = default)
    {
        using var response = await _http.GetAsync($"lego/sets/{Uri.EscapeDataString(setNumber)}/", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LegoSet>(cancellationToken);
    }
}

public sealed record LegoSet(
    [property: JsonPropertyName("set_num")] string SetNum,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("year")] int Year,
    [property: JsonPropertyName("num_parts")] int NumParts);
