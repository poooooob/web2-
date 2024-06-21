﻿using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int Credits { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
