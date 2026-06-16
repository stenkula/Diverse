using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
public interface IClaudeService
{
    Task<string> CategorizeAsync(string todoTitle, string? description = null);
}

public class ClaudeService : IClaudeService
{
    private readonly HttpClient _http;
    private readonly ClaudeOptions _options;

    public ClaudeService(HttpClient http, IOptions<ClaudeOptions> options)
    {
        _http = http;
        _options = options.Value;
        _http.BaseAddress = new Uri("https://api.anthropic.com");
        _http.DefaultRequestHeaders.Add("x-api-key", _options.ApiKey);
        _http.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    }

    public async Task<string> CategorizeAsync(string todoTitle, string? description = null)
    {
        // Lager innholdet for Claude, inkludert både tittel og beskrivelse hvis tilgjengelig
        var userContent = string.IsNullOrEmpty(description)
            ? todoTitle
            : $"{todoTitle}\n{description}";

        var payload = new
        {
            model = _options.Model,
            max_tokens = 100,
            system = """
                Du er en assistent som kategoriserer gjøremål.
                Svar KUN med én kategori fra denne listen:
                Arbeid, Privat, Helse, Økonomi, Sosial, Annet
                
                Ingen forklaring, bare kategorinavnet.
                """,
            messages = new[]
            {
                new { role = "user", content = userContent }
            }
        };

        var response = await _http.PostAsJsonAsync("/v1/messages", payload);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ClaudeResponse>();
        return result?.Content?.FirstOrDefault()?.Text?.Trim() ?? "Annet";
    }
}

// Hjelpeklasser for deserialisering
public record ClaudeResponse(
    [property: JsonPropertyName("content")] List<ContentBlock> Content);

public record ContentBlock(
    [property: JsonPropertyName("text")] string Text);