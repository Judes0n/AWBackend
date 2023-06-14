using System;
using System.Collections.Generic;

namespace AWBackend.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public string Dob { get; set; } = null!;

    public string Course { get; set; } = null!;
}
