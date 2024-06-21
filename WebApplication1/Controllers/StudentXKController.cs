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
        public IActionResult ShowCourse(string searchString, int? page)
        {
            // 将用户名存入ViewData,以便在页面上显示
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            ViewData["CurrentFilter"] = searchString;

            int pageNumber = page ?? 1; // 如果page有值就赋值给pageNumber，否则pageNumber赋值为1


            using (var db = new _2109060310DbContext())
            {
                // 查询课程列表
                var courses = db.Courses.OrderBy(c => c.CourseId).ToPagedList(pageNumber, PageSize);

                if (!string.IsNullOrEmpty(searchString))
                {
                    courses = db.Courses.Where(c => c.CourseName.Contains(searchString)).OrderBy(c => c.CourseId).ToPagedList(pageNumber, PageSize);
                }

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
                    TempData["Message"] = "选课成功!";
                }
                else
                {
                    TempData["Message"] = "你已经选过这门课程!";

                }
                return RedirectToAction("ShowCourse");
            }
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


        //学生个人信息页面
        public IActionResult StudentInfo()
        {
            // 将用户名存入ViewData,以便在页面上显示
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            // 获取当前用户的用户名
            var stuId = HttpContext.Session.GetString("UserName");

            using (var db = new _2109060310DbContext())
            {
                // 查询当前用户的个人信息
                var student = db.Students.Find(int.Parse(stuId));
                return View(student);
            }
        }

        //修改密码
        public IActionResult UpdatePassword(int stuId, string currentPassword, string newPassword)
        {
            using (var db = new _2109060310DbContext())
            {
                var student = db.Students.Find(stuId);
                if (student != null && student.StuInitpwd == currentPassword)
                {
                    student.StuInitpwd = newPassword;
                    db.SaveChanges();
                    TempData["Message"] = "密码更新成功!";
                }
                else
                {
                    TempData["Message"] = "当前密码不正确，密码更新失败!";
                }
            }
            return RedirectToAction("StudentInfo");

        }
    }
}
