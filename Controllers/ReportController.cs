using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiAsync.Models;

namespace WebApiAsync.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {

        private ApplicationContext db;
        public ReportController(ApplicationContext context)
        {
            db = context;
        }


        /* parameter:
         * user - Guid user id
         * from - start interval
         * to   - finish interval         */
        [HttpGet] //TODO replace to Post or delete in release (from test in browser)
        [Route("user_statistics")]
        public async Task<IActionResult> Post()
        {
            var temp = HttpContext.Request.Query["user"].ToString();
            if (string.IsNullOrEmpty(temp) || !Guid.TryParse(temp, out Guid sess))
                return BadRequest("Bad Request");
            temp = HttpContext.Request.Query["from"].ToString();
            if (string.IsNullOrEmpty(temp) || !int.TryParse(temp, out var @from)) from = 0;
            temp = HttpContext.Request.Query["to"].ToString();
            if (string.IsNullOrEmpty(temp) || !int.TryParse(temp, out var to)) to = int.MaxValue;
            User u = await db.Users.FirstOrDefaultAsync(_u => _u.query == sess);
            if (u == null) return Ok(new User());
            var lst = await db.Sessions.ToListAsync();
            lst = lst.Where(usr => usr.Query == sess).ToList();
            int torange = to - from;
            if (torange >= lst.Count) torange = lst.Count - from;

            if (torange <= 0)
                return Ok(new User[0]);
            return
                Ok(lst.GetRange(from, torange).Select(_u => new { query = _u.Query, date_connect = _u.Date }).ToArray());
        }




        /* parameter:
         * user - Guid user id
         * from - start interval : nullable (default=0)
         * to   - finish interval: nulleble (default=count array current sessions)         */
        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> Get()
        {
            var queryUser = HttpContext.Request.Query["user"].ToString();
            if (string.IsNullOrEmpty(queryUser) || !Guid.TryParse(queryUser, out var sess))
                sess = Guid.NewGuid();

            User u = await db.Users.FirstOrDefaultAsync(_u => _u.query == sess);
            if (u == null)
            {
                u = new User { query = sess, persent = 0 };
                db.Users.Add(u);
                db.SaveChanges();
            }

            Session s = await db.Sessions.FirstOrDefaultAsync(_u => _u.UserId == u);
            if (s == null)
            {
                s = new Session { Query = sess, Date = DateTime.Now, UserId = u };
                db.Sessions.Add(s);
                db.SaveChanges();
            }

            if (s.Date == DateTime.MinValue || u.persent >= 100) s.Date = DateTime.Now;

            var p = (u.persent = (DateTime.Now - s.Date).TotalSeconds < Program.Delay
                ? (int)((DateTime.Now - s.Date).TotalSeconds * 100 / Program.Delay)
                : s.Date == DateTime.MinValue? 0: 100);
            if (u.persent >= 100)
            {
                s.CountSingIn++;
                s.Date = DateTime.MinValue;
                db.Sessions.Update(s);
                u.persent = 0;
            }
            db.Users.Update(u);
            await db.SaveChangesAsync();
            var r = new { user_id = u.query, count_sing_in = s.CountSingIn };
            return p >= 100 ?
                Ok(new { query = s.Query.ToString("D"), persent = p, result = r }) : 
                Ok(new { query = s.Query, persent = p, result = (string)null });
        }
    }
}
