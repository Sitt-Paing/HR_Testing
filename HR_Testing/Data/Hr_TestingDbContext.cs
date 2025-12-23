using System;
using System.Collections.Generic;
using Hr_Testing.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hr_Testing.Data;

public partial class Hr_TestingDbContext : DbContext
{
    public Hr_TestingDbContext(DbContextOptions<Hr_TestingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<Township> Townships { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.Property(e => e.BranchId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.DepartmentId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.PositionId).ValueGeneratedNever();
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.Property(e => e.StateId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.Property(e => e.StreetId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Township>(entity =>
        {
            entity.Property(e => e.TownshipId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
