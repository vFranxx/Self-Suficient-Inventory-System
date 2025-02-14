// Aca es donde se configura el contexto de la base de datos
// Se realizan especificaciones de las entidades y sus relaciones

using API.Models.AuditModels;
using API.Models.Entities;
using API.Models.LogModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;


namespace API.Data
{
    public class AppDbContext : IdentityDbContext<SystemOperator>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Main
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<SystemOperator> SystemOperators { get; set; }
        // Logs
        public DbSet<ExceptionLogEntry> ExceptionLogEntries { get; set; }
        public DbSet<ResponseLogEntry> ResponseLogEntries { get; set; }
        // Audit
        public DbSet<ProductAudit> ProductAudits { get; set; }
        public DbSet<BillAudit> BillAudits { get; set; }
        public DbSet<BillDetailAudit> BillDetailAudits { get; set; }
        public DbSet<OrderAudit> OrderAudits { get; set; }
        public DbSet<OrderDetailAudit> OrderDetailAudits { get; set; }
        public DbSet<SupplierAudit> SupplierAudits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(b => b.FacId); // PK

                // Configuración de los atributos
                entity.Property(b => b.FacId)
                      .ValueGeneratedNever();

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
                      .ValueGeneratedNever();

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
                      .ValueGeneratedNever();

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
                      .ValueGeneratedNever();

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
                      .HasColumnType("decimal(12,2)");

                entity.Property(p => p.Descuento)
                      .HasColumnType("decimal(12,2)");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(s => s.ProvId);

                entity.Property(s => s.ProvId)
                      .ValueGeneratedNever();

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

            void ConfigureAudit<T>(EntityTypeBuilder<T> builder) where T : AuditBase
            {
                builder.HasKey(a => a.AuditId);
                builder.Property(a => a.AuditId)
                       .UseIdentityColumn(); // Autoincremental
            }

            // Aplica la configuración a cada auditoría
            ConfigureAudit(modelBuilder.Entity<ProductAudit>());
            ConfigureAudit(modelBuilder.Entity<BillAudit>());
            ConfigureAudit(modelBuilder.Entity<BillDetailAudit>());
            ConfigureAudit(modelBuilder.Entity<OrderAudit>());
            ConfigureAudit(modelBuilder.Entity<OrderDetailAudit>());
            ConfigureAudit(modelBuilder.Entity<SupplierAudit>());
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var userId = GetCurrentUserId();
            var auditEntries = new List<AuditBase>();

            // 1. Primero detectar cambios SIN modificar el contexto
            foreach (var entry in ChangeTracker.Entries().ToList()) // Convertir a lista para iterar
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = CreateAuditEntry(entry, userId);
                if (auditEntry != null) auditEntries.Add(auditEntry);
            }

            // 2. Guardar cambios principales PRIMERO
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // 3. Ahora añadir las auditorías al contexto
            foreach (var auditEntry in auditEntries)
            {
                Add(auditEntry);
            }

            // 4. Guardar auditorías por SEPARADO
            if (auditEntries.Any())
            {
                await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            return result;
        }

        private AuditBase? CreateAuditEntry(EntityEntry entry, string userId)
        {
            var auditType = GetAuditType(entry.Entity.GetType());
            if (auditType == null)
            {
                return null;
            }

            var auditEntry = (AuditBase)Activator.CreateInstance(auditType);
            auditEntry.AuditAction = entry.State.ToString();
            auditEntry.TimeStamp = DateTime.Now;
            auditEntry.UserId = userId;

            PopulateAuditData(entry, auditEntry);

            return auditEntry;
        }

        private void PopulateAuditData(EntityEntry entry, AuditBase auditEntry)
        {
            var entityType = entry.Entity.GetType();
            var values = entry.State == EntityState.Deleted ? entry.OriginalValues : entry.CurrentValues;

            // Copiar valores específicos de la entidad
            foreach (var property in values.Properties)
            {
                if (property.Name.Equals(nameof(AuditBase.AuditId), StringComparison.OrdinalIgnoreCase))
                    continue;

                var value = values[property];
                var auditProperty = auditEntry.GetType().GetProperty(property.Name);
                if (auditProperty != null && auditProperty.CanWrite)
                {
                    auditProperty.SetValue(auditEntry, value);
                }
            }

            // Manejar valores modificados
            if (entry.State == EntityState.Added)
            {
                auditEntry.NewValues = GetCurrentValuesJson(entry);
            }
            else if (entry.State == EntityState.Deleted)
            {
                auditEntry.OriginalValues = GetOriginalValuesJson(entry);
            }
            else if (entry.State == EntityState.Modified)
            {
                var modifiedProps = entry.Properties
                .Where(p => p.IsModified)
                .Select(p => p.Metadata.Name);

                auditEntry.ModifiedColumns = string.Join(",", modifiedProps);
                auditEntry.OriginalValues = GetOriginalValuesJson(entry);
                auditEntry.NewValues = GetCurrentValuesJson(entry);
            }
        }

        private string GetCurrentUserId()
        => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";

        private Type? GetAuditType(Type entityType)
        {
            var auditTypeName = $"{entityType.Name}Audit";
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == auditTypeName && t.Namespace == typeof(AuditBase).Namespace);
        }

        private string? GetCurrentValuesJson(EntityEntry entry)
        {
            return JsonSerializer.Serialize(entry.CurrentValues.ToObject());
        }

        private string? GetOriginalValuesJson(EntityEntry entry)
        => entry.State == EntityState.Modified || entry.State == EntityState.Deleted
            ? JsonSerializer.Serialize(entry.OriginalValues.ToObject())
            : null;
    }
}
