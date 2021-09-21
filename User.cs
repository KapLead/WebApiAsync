using System;

namespace WebApiAsync
{
    public class User : IUser
    {
        public Guid query { get; set; }
        public DateTime Date { get; set; }
        public int persent
        {
            get
            {
                if (Date == DateTime.MinValue)
                {
                    Date = DateTime.Now;
                }
                var persent =  (DateTime.Now - Date).TotalSeconds < Program.Delay
                    ? (int) ((DateTime.Now - Date).TotalSeconds * 100 / Program.Delay) : Date==DateTime.MinValue?0:100;
                if (persent >= 100)
                {
                    count_sing_in++;
                    Date = DateTime.MinValue;
                }
                return persent;
            }
        }
        public int count_sing_in { get; set; }
        public Result result => persent < 100 ? null : new Result
        {
            user_id = this.query.ToString("D"),
            count_sing_in = count_sing_in
        };
    }

    public class UserNUllable : IUser
    {
        public Guid query { get; set; }
        public int persent { get; set; }
        public Result result { get; } = null;

        public UserNUllable(User usr)
        {
            query = usr.query;
            persent = usr.persent;
        }
    }

    public class Result
    {
        public string user_id { get; set; }
        public int count_sing_in { get; set; }
    }
}
