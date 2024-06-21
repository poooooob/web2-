
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult ShowLogin()
        {
            return View();
        }
        [AllowAnonymous]

        [HttpPost]
		//登录操作
		public IActionResult LoginUser(LoginUserModel loginUser)
		{
		   using (var db = new _2109060310DbContext())
            {
                if(loginUser.UserType == "student")
                {
                    var student = db.Students.FirstOrDefault(s => s.StuId.ToString() == loginUser.UserName && s.StuInitpwd == loginUser.Password);
                    if(student != null)
                    {
                        //将用户名存入session
                        HttpContext.Session.SetString("UserName", loginUser.UserName);
                        HttpContext.Session.SetString("UserType", loginUser.UserType);
                        return RedirectToAction("ShowCourse", "StudentXK");
                    }
                    else
                    {
                        ViewData["Message"] = "用户名或密码错误";
                    }
                }
                else
                {
                    if (loginUser.Password == "6")
                    {
                        HttpContext.Session.SetString("UserName", loginUser.UserName);
                        HttpContext.Session.SetString("UserType", loginUser.UserType);
                        return RedirectToAction("ShowAllCourse", "Course");
                    }
                }
            }
           return View("ShowLogin");
		
		}


		public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     

    }
}
