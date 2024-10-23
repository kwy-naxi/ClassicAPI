using BaseAPI.Common.Base;
using BaseAPI.Common.DataTypeUtility;
using BaseAPI.Models;
using BaseAPI.Services.Biz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaseAPI.Controllers
{
    [Authorize(AuthenticationSchemes = ServiceAuthenticationSchemes.JwtOnly)]
    [Area(AreaSchemes.Api)]
    [Route("[area]/User")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        Biz_User biz_user = new Biz_User();

        [AllowAnonymous]
        [HttpPost(Name = "Login")]
        public ActionResult UserLogin([FromForm] User user)
        {
            string json = "";

            DataTable dt = biz_user.GetUser(user.Id, user.Password);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
    }
}
