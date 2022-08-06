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
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<Expense> Expenses { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<ItemTransaction> ItemTransactions { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<PartNumber> PartNumbers { get; set; } = null!;
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;
        public virtual DbSet<ReceivedItem> ReceivedItems { get; set; } = null!;
        public virtual DbSet<ReceivedItemDetail> ReceivedItemDetails { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<TransactionType> TransactionTypes { get; set; } = null!;
        public virtual DbSet<Uom> Uoms { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-OVUJU65;Initial Catalog=accounting_system;Persist Security Info=False;User ID=sa;Password=123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
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

                entity.Property(e => e.ReorderPoint).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Item_Brand");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Item_Category");

                entity.HasOne(d => d.PartNumber)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.PartNumberId)
                    .HasConstraintName("FK_Item_PartNumber");

                entity.HasOne(d => d.Uom)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.UomId)
                    .HasConstraintName("FK_Item_Uom");
            });

            modelBuilder.Entity<ItemTransaction>(entity =>
            {
                entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemTransactions)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ItemTransactions_Item");

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.ItemTransactions)
                    .HasForeignKey(d => d.TransactionTypeId)
                    .HasConstraintName("FK_ItemTransactions_TransactionType");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Location_Category");
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

            modelBuilder.Entity<ReceivedItem>(entity =>
            {
                entity.ToTable("ReceivedItem");

                entity.Property(e => e.DateReceived).HasColumnType("datetime");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.ReceivedItems)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_ReceivedItem_Supplier");
            });

            modelBuilder.Entity<ReceivedItemDetail>(entity =>
            {
                entity.ToTable("ReceivedItemDetail");

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ReceivedItemDetails)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ReceivedItemDetail_Item");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.ReceivedItemDetails)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_ReceivedItemDetail_Location");

                entity.HasOne(d => d.ReceivedItem)
                    .WithMany(p => p.ReceivedItemDetails)
                    .HasForeignKey(d => d.ReceivedItemId)
                    .HasConstraintName("FK_ReceivedItemDetail_ReceivedItem");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.ToTable("TransactionType");
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
