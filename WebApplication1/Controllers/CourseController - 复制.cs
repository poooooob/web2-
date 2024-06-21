
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCourese()
		{

            return View();
		
		}

		public IActionResult ShowAllCourse()
		{
            //将用户名存入ViewData,以便在页面上显示
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
			//查询课程列表
			using (var db = new _2109060310DbContext())
			{
				List<Course> courses = db.Courses.ToList();
				return View(courses);
			}
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
