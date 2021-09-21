using System;

namespace WebApiAsync
{
    public interface IUser{
        Guid query {get; set;}
        int persent { get; }
        Result result { get; }
    }
}
