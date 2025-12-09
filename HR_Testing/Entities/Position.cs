using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Entities;

[Table("Position")]
public partial class Position
{
    [Key]
    [Column("positionId")]
    public int PositionId { get; set; }

    [Column("positionName")]
    public string? PositionName { get; set; }

    [Column("departmentId")]
    public int? DepartmentId { get; set; }

    [Column("status")]
    public bool? Status { get; set; }

    [Column("createdOn", TypeName = "datetime")]
    public DateTime? CreatedOn { get; set; }

    [Column("updatedOn", TypeName = "datetime")]
    public DateTime? UpdatedOn { get; set; }

    [Column("deletedOn", TypeName = "datetime")]
    public DateTime? DeletedOn { get; set; }
}
