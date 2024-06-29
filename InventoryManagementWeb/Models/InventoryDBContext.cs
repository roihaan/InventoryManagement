using Microsoft.EntityFrameworkCore;

namespace InventoryManagementWeb.Models
{
    public partial class InventoryDBContext : DbContext
    {
        public InventoryDBContext(DbContextOptions<InventoryDBContext> options)
            : base(options)
        {
        }

        // DbSet properties for entities
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        // Configuring the DbContext options
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Ensure to configure the connection string properly
            optionsBuilder.UseSqlServer("Server=.;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        // Configuring the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define entity configurations
            modelBuilder.Entity<Product>(entity =>
            {
                // Optional: Explicitly define table name
                entity.ToTable("Products");

                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDAEA23590");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                // Optional: Explicitly define table name
                entity.ToTable("Transactions");

                entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B63DDBB3A");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
                entity.Property(e => e.Date)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.TransactionType)
                      .HasMaxLength(10)
                      .IsRequired(); // Ensure TransactionType is required

                // Define relationships
                entity.HasOne(d => d.Product)
                      .WithMany(p => p.Transactions)
                      .HasForeignKey(d => d.ProductId)
                      .HasConstraintName("FK__Transacti__Produ__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
