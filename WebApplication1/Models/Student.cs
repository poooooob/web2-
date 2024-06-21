using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Student
{
    public int StuId { get; set; }

    public string StuName { get; set; } = null!;

    public string StuClass { get; set; } = null!;

    public string StuInitpwd { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();


}
