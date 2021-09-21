using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAsync.Models
{
    public class Session
    {
        [Key] public Guid query { get; set; }
        public DateTime date { get; set; }
        public int count_sing_in { get; set; }
    }
}
