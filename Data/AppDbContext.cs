// Aca es donde se configura el contexto de la base de datos
// Se realizan especificaciones de las entidades y sus relaciones

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RESTful_API.Models.Entities;
using Self_Suficient_Inventory_System.Models.AuditModels;
using Self_Suficient_Inventory_System.Models.LogModels;

namespace RESTful_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<SystemOperator> SystemOperators { get; set; }
        public DbSet<ExceptionLogEntry> ExceptionLogEntries { get; set; }
        public DbSet<ResponseLogEntry> ResponseLogEntries { get; set; }
        public DbSet<ProductAudit> ProductAudits { get; set; }
        public DbSet<SupplierAudit> SupplierAudits { get; set; }
        public DbSet<SystemOperatorAudit> SystemOperatorAudits { get; set; }
        public DbSet<BillAudit> BillAudits { get; set; }
        public DbSet<BillDetailAudit> BillDetailAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(b => b.FacId); // PK

                // Configuración de los atributos
                entity.Property(b => b.FacId)
                      .UseIdentityColumn(); // Identity

                entity.Property(b => b.Total)
                      .HasColumnType("decimal(18,2)");

                // Configuración FK
                entity.HasOne(b => b.Operators)
                      .WithMany()
                      .HasForeignKey(b => b.IdOp)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.HasKey(bd => bd.FacDetId);

                entity.Property(bd => bd.FacDetId)
                      .UseIdentityColumn();

                entity.Property(bd => bd.Precio)
                      .HasColumnType("decimal(12,2)");

                entity.Property(bd => bd.Subtotal)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(bd => bd.Facturas)
                      .WithMany()
                      .HasForeignKey(bd => bd.IdFactura)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bd => bd.Productos)
                      .WithMany()
                      .HasForeignKey(bd => bd.IdProducto)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OcId);

                entity.Property(o => o.OcId)
                      .UseIdentityColumn();

                entity.Property(o => o.Estado)
                      .HasMaxLength(20);

                entity.HasOne(o => o.Operators)
                      .WithMany()
                      .HasForeignKey(o => o.IdOp)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Suppliers)
                      .WithMany()
                      .HasForeignKey(o => o.IdProv)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(od => od.DetOcId);

                entity.Property(od => od.DetOcId)
                      .UseIdentityColumn();

                entity.HasOne(od => od.Orders)
                      .WithMany()
                      .HasForeignKey(od => od.IdOc)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(od => od.Products)
                      .WithMany()
                      .HasForeignKey(od => od.IdProd)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProdId);

                entity.Property(p => p.ProdId)
                      .HasMaxLength(13);

                entity.Property(p => p.Descripcion)
                      .HasMaxLength(60);

                entity.Property(p => p.PrecioUnitario)
                      .HasColumnType("decimal(12,2)");

                entity.Property(p => p.Ganancia)
                      .HasColumnType("decimal(5,2)");

                entity.Property(p => p.Descuento)
                      .HasColumnType("decimal(2,1)");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(s => s.ProvId);

                entity.Property(s => s.ProvId)
                      .UseIdentityColumn();

                entity.Property(s => s.Referencia)
                      .HasMaxLength(80);

                entity.Property(s => s.Contacto)
                      .HasMaxLength(20);

                entity.Property(s => s.Mail)
                      .HasMaxLength(80);

                entity.Property(s => s.Direccion)
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<SupplierProduct>(entity =>
            {
                entity.HasKey(sp => new { sp.IdProv, sp.IdProd });

                entity.HasOne(sp => sp.Suppliers)
                      .WithMany()
                      .HasForeignKey(sp => sp.IdProv) 
                      .OnDelete(DeleteBehavior.Restrict); 

                entity.HasOne(sp => sp.Products)
                      .WithMany()
                      .HasForeignKey(sp => sp.IdProd) 
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SystemOperator>(entity =>
            {
                entity.HasKey(op => op.Uid);

                entity.Property(op => op.Uid)
                      .HasMaxLength(50);

                entity.Property(op => op.Nombre)
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<ExceptionLogEntry>(ExceptionLogEntries =>
            {
                ExceptionLogEntries.HasKey(l => l.Id);

                ExceptionLogEntries.Property(l => l.Id)
                          .UseIdentityColumn();
                
                ExceptionLogEntries.Property(l => l.StatusCode)
                          .IsRequired(false);
            });
            
            modelBuilder.Entity<ResponseLogEntry>(ResponseLogEntries =>
            {
                ResponseLogEntries.HasKey(l => l.Id);

                ResponseLogEntries.Property(l => l.Id)
                          .UseIdentityColumn();

                ResponseLogEntries.Property(l => l.StatusCode)
                          .IsRequired(false);
            });

            modelBuilder.Entity<ProductAudit>(ProductAudits =>
            {
                ProductAudits.HasKey(a => a.AuditId);

                ProductAudits.Property(a => a.AuditId)
                          .UseIdentityColumn();
            });

            modelBuilder.Entity<SupplierAudit>(SupplierAudits =>
            {
                SupplierAudits.HasKey(a => a.AuditId);

                SupplierAudits.Property(a => a.AuditId)
                          .UseIdentityColumn();
            });

            modelBuilder.Entity<SystemOperatorAudit>(SystemOperatorAudits =>
            {
                SystemOperatorAudits.HasKey(a => a.AuditId);

                SystemOperatorAudits.Property(a => a.AuditId)
                          .UseIdentityColumn();
            });

            modelBuilder.Entity<BillAudit>(BillAudits =>
            {
                BillAudits.HasKey(a => a.AuditId);

                BillAudits.Property(a => a.AuditId)
                          .UseIdentityColumn();
            });

            modelBuilder.Entity<BillDetailAudit>(BillDetailAudits =>
            {
                BillDetailAudits.HasKey(a => a.AuditId);

                BillDetailAudits.Property(a => a.AuditId)
                          .UseIdentityColumn();
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await AuditProductChangesAsync();

            await AuditSupplierChangesAsync();

            await AuditSystemOperatorChangesAsync();

            await AuditBillChangesAsync();

            await AuditBillDetailChangesAsync();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task AuditProductChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<Product>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear un registro de auditoría para cada cambio
                var productAudit = new ProductAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = "",  // Placeholder
                    ProdId = (string)originalValues["ProdId"]!,
                    Descripcion = (string)originalValues["Descripcion"]!,
                    PrecioUnitario = (decimal)originalValues["PrecioUnitario"]!,
                    Ganancia = (decimal)originalValues["Ganancia"]!,
                    Descuento = (decimal?)originalValues["Descuento"],
                    Stock = (int?)originalValues["Stock"],
                    StockMin = (int?)originalValues["StockMin"],
                    FechaBaja = (DateTime?)originalValues["FechaBaja"]
                };

                // Añadir el registro de auditoría asincrónicamente
                await ProductAudits.AddAsync(productAudit);
            }
        }

        private async Task AuditSupplierChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<Supplier>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new SupplierAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = "",  // Placeholder
                    Referencia = (string)originalValues["Referencia"],
                    Contacto = (string?)originalValues["Contacto"],
                    Direccion = (string?)originalValues["Direccion"],
                    Mail = (string?)originalValues["Mail"]
                };

                await SupplierAudits.AddAsync(audit);
            }
        }

        private async Task AuditSystemOperatorChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<SystemOperator>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new SystemOperatorAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = "",  // Placeholder
                    Nombre = (string?)originalValues["Nombre"],
                    Tipo = (bool)originalValues["Tipo"],
                    Pswd = (string?)originalValues["Pswd"],
                    FechaBaja = (DateTime)originalValues["FechaBaja"]
                };

                await SystemOperatorAudits.AddAsync(audit);
            }
        }

        private async Task AuditBillChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<Bill>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new BillAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = "",  // Placeholder
                    FechaHora = (DateTime)originalValues["FechaHora"],
                    Total = (decimal)originalValues["Total"],
                    IdOp = (string)originalValues["IdOp"]
                };

                await BillAudits.AddAsync(audit);
                
            }
        }

        private async Task AuditBillDetailChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<BillDetail>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new BillDetailAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = "",  // Placeholder
                    Cantidad = (int)originalValues["Cantidad"],
                    Precio = (decimal)originalValues["Precio"],
                    Subtotal = (decimal)originalValues["Subtotal"],
                    IdFactura = (int)originalValues["IdFactura"],
                    IdProducto = (string)originalValues["IdProducto"]
                };

                await BillDetailAudits.AddAsync(audit);
            }
        }

    }
}
