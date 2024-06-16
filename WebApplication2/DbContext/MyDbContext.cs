using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication1.DatabaseContext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CountSheet> CountSheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure CountSheet table
            modelBuilder.Entity<CountSheet>(entity =>
            {
                entity.ToTable("count_sheet");

                entity.HasKey(e => e.CountId);

                entity.Property(e => e.CountId).HasColumnName("cou_id");
                entity.Property(e => e.CountCode).HasColumnName("cou_code").HasMaxLength(50).IsRequired();
                entity.Property(e => e.CountName).HasColumnName("cou_name").HasMaxLength(100).IsRequired();
            });

            // Configure Employee table
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee_count");

                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId).HasColumnName("emp_id");
                entity.Property(e => e.EmployeeCode).HasColumnName("emp_code").HasMaxLength(50).IsRequired();
                entity.Property(e => e.EmployeeName).HasColumnName("emp_name").HasMaxLength(100).IsRequired();
            });

            // Configure Inventory (ItemCount) table
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("item_count");

                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.ItemId).HasColumnName("itm_id");
                entity.Property(e => e.ItemNo).HasColumnName("itm_no").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ItemDescription).HasColumnName("itm_description").HasMaxLength(100).IsRequired();
                entity.Property(e => e.ItemUom).HasColumnName("itm_uom").HasMaxLength(20).IsRequired();
                entity.Property(e => e.ItemLotNumber).HasColumnName("itm_lot").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ItemExpiry).HasColumnName("itm_expiry").HasMaxLength(20).IsRequired();
                entity.Property(e => e.ItemQuantity).HasColumnName("itm_quantity").IsRequired();
                entity.Property(e => e.ItemDateLog).HasColumnName("itm_date_log").HasMaxLength(20).IsRequired();
                entity.Property(e => e.ItemCouCode).HasColumnName("itm_cou_code").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ItemEmpCode).HasColumnName("itm_emp_code").HasMaxLength(50).IsRequired();
            });
        }
    }
}
