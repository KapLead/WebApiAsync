using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiAsync.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {

        private readonly ILogger<ReportController> _logger;

        public ReportController(ILogger<ReportController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IUser Get()
        {
            var user = new IUser
            {
                query = Guid.NewGuid(),
                persent=0,
                result = null
            };
            return user;
        }
    }
}
