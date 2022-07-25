using System;
using System.Collections.Generic;
using AccountingSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AccountingSystem.Server.DataAccess
{
    public partial class accounting_systemContext : DbContext
    {
        public accounting_systemContext()
        {
        }

        public accounting_systemContext(DbContextOptions<accounting_systemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branches { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<Expense> Expenses { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<PartNumber> PartNumbers { get; set; } = null!;
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
        public virtual DbSet<Uom> Uoms { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=AQUIP-CEBU;Initial Catalog=accounting_system;Persist Security Info=False;User ID=sa;Password=Hello@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("Branch");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");

                entity.Property(e => e.DateEnt).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.ToTable("Expense");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_Expense_Contract");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.PartNumber)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.PartNumberId)
                    .HasConstraintName("FK_Item_PartNumber");

                entity.HasOne(d => d.Uom)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.UomId)
                    .HasConstraintName("FK_Item_Uom");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PartNumber>(entity =>
            {
                entity.ToTable("PartNumber");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("PaymentTransaction");

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.PaymentTransactions)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_PaymentTransaction_Contract");
            });

            modelBuilder.Entity<Uom>(entity =>
            {
                entity.ToTable("Uom");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
