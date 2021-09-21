using System;

namespace WebApiAsync.Models
{
    public class UserNUllable : IUser
    {
        public Guid query { get; set; }
        public int persent { get; set; }
        public Result result { get; set; } = null;
    }
}
