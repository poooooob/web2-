
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
        //查询课程信息
		public IActionResult CourseEdit(string id) {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
			//根据id查询课程信息
            using (var db = new _2109060310DbContext())
			{
				var course = db.Courses.Find(int.Parse(id));
                return View("CourseEdit", course);
			}
		}
        //保存课程信息
        [HttpPost]
        public IActionResult SaveCourse(Course course)
		{
		    using(var db = new _2109060310DbContext())
            {
                var model = db.Courses.Find(course.CourseId);
                if(model== null)
                {
                    model = new Course();
                    model.CourseId = course.CourseId;
                    model.CourseName = course.CourseName;
                    model.Credits= course.Credits;
                    db.Courses.Add(model);
                }
                else
                {
					model.CourseName = course.CourseName;
					model.Credits = course.Credits;
				}
                db.SaveChanges();
            }
        return RedirectToAction("ShowAllCourse","Course");
		}
        //新增课程
        // GET: CourseAdd
        [HttpGet]
        public IActionResult CourseAdd()
        {
            return View();
        }

        // POST: CourseAdd
        [HttpPost]
        public IActionResult CourseAdd(Course course)
        {
            using (var db = new _2109060310DbContext())
            {
                db.Courses.Add(course);
                db.SaveChanges();
            }
            return RedirectToAction("ShowAllCourse", "Course");
        }



        // 显示删除确认页面
        [HttpGet]
        public IActionResult CourseDelete(int id)
        {
            using (var db = new _2109060310DbContext())
            {
                var course = db.Courses.Find(id);
                if (course == null)
                {
                    return NotFound();
                }
                return View(course);
            }
        }
        //删除课程信息
        [HttpPost]
        public IActionResult ConfirmDelete(string id)
        {
            //
            using (var db = new _2109060310DbContext())
            {
                var course = db.Courses.Find(int.Parse(id));

                // 检查是否有学生选了该课程
                var enrollments = db.Enrollments.Where(e => e.CourseId == int.Parse(id));
                if (enrollments.Any())
                {
                    // 如果有学生选了该课程，返回错误信息
                    ModelState.AddModelError(string.Empty, "该课程已经被人选啦，不能删除");
                    return View("CourseDelete", course);
                }


                db.Courses.Remove(course);
                db.SaveChanges();
            }
            return RedirectToAction("ShowAllCourse", "Course");

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
