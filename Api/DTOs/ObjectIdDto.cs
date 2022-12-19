using System.Text.Json.Serialization;

namespace Api.DTOs
{
    public class ObjectIdDto
    {
        [JsonPropertyName("$oid")]
        public string oid { get; set; }
    }
}
