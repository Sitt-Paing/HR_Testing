using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Entities;

[Table("UserInfo")]
public partial class UserInfo
{
    [Key]
    [StringLength(256)]
    public string UserId { get; set; } = null!;

    public string? FullName { get; set; }

    public bool? Status { get; set; }

    public DateOnly? JoinDate { get; set; }

    public int? StateId { get; set; }

    public int? TownshipId { get; set; }

    public int? StreetId { get; set; }

    [StringLength(500)]
    public string? ProfileImage { get; set; }
}
