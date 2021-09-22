using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace WebApiAsync.Models
{
    public class Session
    {
        [Key, XmlIgnore, JsonIgnore] public int SessionId { get; set; }
        [JsonPropertyName("query")] public Guid Query { get; set; }
        
        [JsonPropertyName("date")] public DateTime Date { get; set; }
  
        [JsonPropertyName("count_sing_in")] public int CountSingIn { get; set; }
        public virtual User UserId { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
