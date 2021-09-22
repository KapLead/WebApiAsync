using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace WebApiAsync.Models
{
    public class User
    {
        [Key,XmlIgnore,JsonIgnore] public int UserId { get; set; }
        public Guid query { get; set; }
        public int persent { get; set; }
    }

}
