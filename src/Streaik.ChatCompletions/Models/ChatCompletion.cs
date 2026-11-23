using System.Text.Json.Serialization;
using System.Text.Json;

namespace Streaik.ChatCompletions.Models;

public class ChatCompletion
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("object")]
    public string Object { get; set; } = "chat.completion";

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = default!;

    [JsonPropertyName("choices")]
    public IEnumerable<ChatCompletionChoice> Choices { get; set; } = [];

    [JsonPropertyName("usage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChatCompletionUsage? Usage { get; set; }

    [JsonPropertyName("service_tier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ServiceTier { get; set; }

    [JsonPropertyName("system_fingerprint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SystemFingerprint { get; set; }

    [JsonIgnore]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>Non-string metadata, such as a gateway cost estimate, without changing the string metadata API.</summary>
    [JsonIgnore]
    public Dictionary<string, JsonElement>? MetadataExtensions { get; set; }

    [JsonInclude]
    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    private Dictionary<string, JsonElement>? WireMetadata
    {
        get
        {
            if (Metadata is null) return MetadataExtensions;
            var result = MetadataExtensions is null ? new Dictionary<string, JsonElement>() : new(MetadataExtensions);
            foreach (var (name, value) in Metadata) result[name] = JsonSerializer.SerializeToElement(value);
            return result;
        }
        set
        {
            Metadata = value is null ? null : new Dictionary<string, string>();
            MetadataExtensions = null;
            foreach (var (name, entry) in value ?? [])
                if (entry.ValueKind is JsonValueKind.String or JsonValueKind.Null) Metadata![name] = entry.GetString()!;
                else (MetadataExtensions ??= [])[name] = entry;
        }
    }

    [JsonPropertyName("moderation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChatCompletionModeration? Moderation { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}


public class ChatCompletionUpdate
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("object")]
    public string Object { get; set; } = "chat.completion.chunk";

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = default!;

    [JsonPropertyName("service_tier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ServiceTier { get; set; }

    [JsonPropertyName("system_fingerprint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SystemFingerprint { get; set; }

    [JsonPropertyName("choices")]
    public IEnumerable<ChatCompletionStreamChoice> Choices { get; set; } = [];

    [JsonPropertyName("usage")]
    public ChatCompletionUsage? Usage { get; set; }

    [JsonPropertyName("moderation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ChatCompletionModeration? Moderation { get; set; }

    [JsonPropertyName("obfuscation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Obfuscation { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}
