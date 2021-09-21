using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAsync.Models
{
    public class Result
    {
        [Key] public Guid user_id { get; set; }
        public int count_sing_in { get; set; }
    }
}
