using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Entities;

[Table("Company")]
public partial class Company
{
    [Key]
    [StringLength(50)]
    public string CompanyId { get; set; } = null!;

    public string? CompanyName { get; set; }

    public DateOnly? JoinDate { get; set; }

    [StringLength(50)]
    public string? PrimaryPhone { get; set; }

    [StringLength(50)]
    public string? OtherPhone { get; set; }

    public string? Email { get; set; }
}
