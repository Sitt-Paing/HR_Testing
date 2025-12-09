using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Entities;

[Table("Department")]
public partial class Department
{
    [Key]
    [Column("departmentId")]
    public int DepartmentId { get; set; }

    [Column("departmentName")]
    public string? DepartmentName { get; set; }

    [Column("status")]
    public bool? Status { get; set; }
}
