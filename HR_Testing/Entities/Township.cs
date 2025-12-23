using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Entities;

[Table("Township")]
public partial class Township
{
    [Key]
    public int TownshipId { get; set; }

    public int? StateId { get; set; }

    public string? TownshipName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedAt { get; set; }
}
