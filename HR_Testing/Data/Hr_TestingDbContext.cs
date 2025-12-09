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

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<township> Townships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<township>(entity =>
        {
            entity.Property(e => e.TownshipId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
