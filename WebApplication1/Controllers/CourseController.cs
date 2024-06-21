
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
        //��ѯ�γ���Ϣ
		public IActionResult CourseEdit(string id) {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
			//����id��ѯ�γ���Ϣ
            using (var db = new _2109060310DbContext())
			{
				var course = db.Courses.Find(int.Parse(id));
                return View("CourseEdit", course);
			}
		}
        //����γ���Ϣ
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
        //�����γ�
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



        // ��ʾɾ��ȷ��ҳ��
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
        //ɾ���γ���Ϣ
        [HttpPost]
        public IActionResult ConfirmDelete(string id)
        {
            //
            using (var db = new _2109060310DbContext())
            {
                var course = db.Courses.Find(int.Parse(id));

                // ����Ƿ���ѧ��ѡ�˸ÿγ�
                var enrollments = db.Enrollments.Where(e => e.CourseId == int.Parse(id));
                if (enrollments.Any())
                {
                    // �����ѧ��ѡ�˸ÿγ̣����ش�����Ϣ
                    ModelState.AddModelError(string.Empty, "�ÿγ��Ѿ�����ѡ��������ɾ��");
                    return View("CourseDelete", course);
                }


                db.Courses.Remove(course);
                db.SaveChanges();
            }
            return RedirectToAction("ShowAllCourse", "Course");

        }

		public IActionResult ShowAllCourse()
		{
            //���û�������ViewData,�Ա���ҳ������ʾ
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
			//��ѯ�γ��б�
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
