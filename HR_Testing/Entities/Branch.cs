using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Entities;

[Table("Branch")]
public partial class Branch
{
    [Key]
    [Column("branchId")]
    public int BranchId { get; set; }

    [Column("branchName")]
    public string? BranchName { get; set; }

    [Column("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("companyId")]
    public int? CompanyId { get; set; }

    [Column("status")]
    public bool? Status { get; set; }

    [Column("createdOn", TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    [Column("updatedOn", TypeName = "datetime")]
    public DateTime? UpdatedOn { get; set; }

    [Column("deletedOn", TypeName = "datetime")]
    public DateTime? DeletedOn { get; set; }
}
