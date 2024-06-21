using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Controllers
{
    public class StudentXKController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private const int PageSize = 5;

        // 展示可选课程
        public IActionResult ShowCourse(int? page)
        {
            // 将用户名存入ViewData,以便在页面上显示
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            int pageNumber = page ?? 1; // 如果page有值就赋值给pageNumber，否则pageNumber赋值为1


            using (var db = new _2109060310DbContext())
            {
                // 查询课程列表
                var courses = db.Courses.OrderBy(c => c.CourseId).ToPagedList(pageNumber, PageSize);
                
                return View(courses);
            }
        }

        //显示当前登录用户的选课情况
        public IActionResult ShowStudentXK()
        {
            // 将用户名存入ViewData,以便在页面上显示
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            // 获取当前用户的用户名
            var stuId = HttpContext.Session.GetString("UserName");

            using (var db = new _2109060310DbContext())
            {
                // 查询当前用户的选课情况
                List<Enrollment> enrollments = db.Enrollments
                                                  .Include(e => e.Course)
                                                  .Where(e => e.StuId == int.Parse(stuId))
                                                  .ToList();
                //转换为课程列表
                var courses = enrollments.Select(e => e.Course).ToList();
                return View(courses);

            }
        }




        //选课
        public IActionResult StudentXK(int CourseId)
        {
            using (var db = new _2109060310DbContext())
            {
                var course = db.Courses.Find(CourseId);
                return View(course);
            }
        }

        //确认选课
        public IActionResult ConfirmXK(int? courseId)
        {
            // 获取当前用户的用户名
            var stuId = HttpContext.Session.GetString("UserName");
            // 查询当前用户的选课情况
            using (var db = new _2109060310DbContext())
            {
                var enrollments = db.Enrollments
                                    .Where(e => e.StuId == int.Parse(stuId) && e.CourseId == courseId)
                                    .ToList();
                // 如果当前用户没有选过这门课
                if (enrollments.Count == 0)
                {
                    // 添加选课记录
                    db.Enrollments.Add(new Enrollment
                    {
                        CourseId = courseId,
                        StuId = int.Parse(stuId)
                    });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ShowStudentXK");
        }

     
        // 展示确认删除选课页面
        public IActionResult DeleteXK(int courseId)
        {
           using (var db = new _2109060310DbContext())
            {
                var course = db.Courses.Find(courseId);
                return View(course);
            }

        }

        // 删除选课
        public IActionResult ConfirmDeleteXK(int courseId)
        {
            var stuId = HttpContext.Session.GetString("UserName");
            using (var db = new _2109060310DbContext())
            {
                var enrollment = db.Enrollments
                                   .Where(e => e.StuId == int.Parse(stuId) && e.CourseId == courseId)
                                   .FirstOrDefault();
                if (enrollment != null)
                {
                    db.Enrollments.Remove(enrollment);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ShowStudentXK");
        }


    }
}
