using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAsync.Models
{
    public class User : IUser
    {
        [Key] public Guid query { get; set; }
        public int persent { get; set; }
        public Result result { get; set; } = null;
    }

}
