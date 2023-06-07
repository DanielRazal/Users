
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Users_Server.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; } = null!;

    }
}