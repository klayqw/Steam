using System.Text.Json.Serialization;

namespace Steam.Dto;

public class MessageDto
{
    public string UserId {  get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("groupId")]
    public int GroupId { get; set; }
}
