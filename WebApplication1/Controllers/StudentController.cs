using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class StudentController : Controller
	{
       

        //展示学生列表
        public IActionResult ShowAllStudent()
        {
            //将用户名存入ViewData,以便在页面上显示
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            //查询学生列表
            using (var db = new _2109060310DbContext())
            {
                List<Student> Student = db.Students.ToList();
                return View(Student);
            }
        }

        //查询学生信息
        public IActionResult StudentEdit(int id)
        {
        
            using (var db = new _2109060310DbContext())
            {
                var student = db.Students.Find(id);
                return View("StudentEdit", student);
            }
        }

        [HttpPost]
        public IActionResult SaveStudent(Student student)
        {
           using(var db = new _2109060310DbContext())
            {
                var model = db.Students.Find(student.StuId);
                if(model == null)
                {
                    model = new Student();
                    model.StuId = student.StuId;
                    model.StuName = student.StuName;
                    model.StuClass = student.StuClass;
                    model.StuInitpwd = student.StuInitpwd;
                    db.Students.Add(model);
                }
                else
                {
                    model.StuName = student.StuName;
                    model.StuClass = student.StuClass;
                    model.StuInitpwd = student.StuInitpwd;
                }
                db.SaveChanges();
            }
           return RedirectToAction("ShowAllStudent","Student");
        }

        //删除学生
        [HttpGet]
        public IActionResult StudentDelete(int id)
        {
            using (var db = new _2109060310DbContext())
            {
                var Student = db.Students.Find(id);
                if (Student == null)
                {
                    return NotFound();
                }
                return View(Student);
            }
        }
        //确认删除学生
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            using (var db = new _2109060310DbContext())
            {
                var student = db.Students.Find(id);

                // 删除该学生的所有选课记录
                var enrollments = db.Enrollments.Where(e => e.StuId == id);
                db.Enrollments.RemoveRange(enrollments);

                //删除学生
                db.Students.Remove(student);
                db.SaveChanges();
            } 

            return RedirectToAction("ShowAllStudent","Student");
        }

        //新增学生
        [HttpGet]
        public IActionResult StudentAdd()
        {
            return View();
        }

        // POST: CourseAdd
        [HttpPost]
        public IActionResult StudentAdd(Student student)
        {
            // 检查数据库中是否已经存在相同的StuId
            var db = new _2109060310DbContext();
            var existingStudent = db.Students.FirstOrDefault(s => s.StuId == student.StuId);
            if (existingStudent != null)
            {
                ModelState.AddModelError("StuId", "该学号已经存在，请使用其他学号。");
                return View(student);
            }

            // 如果不存在，继续添加学生
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("ShowAllStudent","Student");
            }

            return View(student);
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
