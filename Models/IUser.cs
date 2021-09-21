using System;

namespace WebApiAsync.Models
{
    public interface IUser
    {
        Guid query {get; set;}
        int persent { get; }
        Result result { get; }
    }
}
