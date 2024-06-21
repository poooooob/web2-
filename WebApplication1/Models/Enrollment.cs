using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int? CourseId { get; set; }

    public int? StuId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Stu { get; set; }
}
