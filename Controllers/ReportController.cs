using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        [Route("user_statistics")]
        public async Task<IActionResult> Post()
        {
            var temp = HttpContext.Request.Query["user"].ToString();
            if (string.IsNullOrEmpty(temp) || !Guid.TryParse(temp, out Guid sess))
                return BadRequest("Bad Request");
            temp = HttpContext.Request.Query["from"].ToString();
            if (string.IsNullOrEmpty(temp) || !int.TryParse(temp, out var @from)) from=0;
            temp = HttpContext.Request.Query["to"].ToString();
            if (string.IsNullOrEmpty(temp) || !int.TryParse(temp, out var to)) to = int.MaxValue;
            User u = await db.Users.FirstOrDefaultAsync(_u=>_u.query==sess);
            if (u == null) return Ok(new User());
            var lst = await db.Sessions.ToListAsync();
            lst = lst.Where(usr => usr.query == sess).ToList();
            int torange = to - from;
            if (torange >= lst.Count) torange = lst.Count - to;
            if(torange<=0)
                return Ok(new User[0]);
            return Ok(lst.GetRange(from,torange));
        }


        /* parameter:
         * user - Guid user id
         * from - start interval
         * to   - finish interval         */
        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> Get()
        {
            var queryUser = HttpContext.Request.Query["user"].ToString(); 
            if(string.IsNullOrEmpty(queryUser) || Guid.TryParse(queryUser,out var sess))
                sess = Guid.NewGuid();

            User u = await db.Users.FirstOrDefaultAsync(_u => _u.query == sess);
            if (u == null)
            {
                u = new User {query = sess, persent = 0, result = null};
                await db.Users.AddAsync(u);
                await db.SaveChangesAsync();
            }

            Session s = await db.Sessions.FirstOrDefaultAsync(_u => _u.query == sess);
            if (s == null)
            {
                s = new Session {query = sess, date = DateTime.Now};
                await db.Sessions.AddAsync(s);
                await db.SaveChangesAsync();
            }

            if (s.date == DateTime.MinValue) s.date = DateTime.Now;

            u.persent = (DateTime.Now - s.date).TotalSeconds < Program.Delay
                ? (int)((DateTime.Now - s.date).TotalSeconds * 100 / Program.Delay) 
                : s.date == DateTime.MinValue ? 0 : 100;

            if (u.persent >= 100)
            {
                s.count_sing_in++;
                s.date = DateTime.MinValue;
                u.result = new Result { user_id = u.query, count_sing_in = s.count_sing_in };
            }
            else u.result = null;
            return Ok(u);
        }
    }
}
