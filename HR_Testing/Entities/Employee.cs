using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Entities;

[Table("employee")]
public partial class Employee
{
    [Key]
    [Column("employeeId")]
    public int EmployeeId { get; set; }

    [Column("fullName")]
    public string? FullName { get; set; }

    [Column("joinDate")]
    public DateOnly? JoinDate { get; set; }

    [Column("dateOfBirth")]
    public DateOnly? DateOfBirth { get; set; }

    [Column("gender")]
    [StringLength(50)]
    public string? Gender { get; set; }

    [Column("nrc")]
    public string? Nrc { get; set; }
}
