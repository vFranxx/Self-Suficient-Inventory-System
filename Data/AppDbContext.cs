// Aca es donde se configura el contexto de la base de datos
// Se realizan especificaciones de las entidades y sus relaciones

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RESTful_API.Models.Entities;
using Self_Suficient_Inventory_System.Models.AuditModels;
using Self_Suficient_Inventory_System.Models.LogModels;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Self_Suficient_Inventory_System.Services;
using Microsoft.AspNetCore.Identity;


namespace RESTful_API.Data
{
    public class AppDbContext : IdentityDbContext<SystemOperator>
    {
        private readonly UserManager<SystemOperator> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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
        public DbSet<AuditBase> Audits { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            // Configuración TPH CORREGIDA
            modelBuilder.Entity<AuditBase>()
                .ToTable("Audits")
                .HasDiscriminator<string>(a => a.AuditType)
                .HasValue<ProductAudit>("ProductAudit")
                .HasValue<SupplierAudit>("SupplierAudit")
                .HasValue<BillAudit>("BillAudit")              
                .HasValue<BillDetailAudit>("BillDetailAudit") 
                .HasValue<OrderAudit>("OrderAudit")      
                .HasValue<OrderDetailAudit>("OrderDetailAudit") 
                .HasValue<SystemOperatorAudit>("SystemOperatorAudit");

            modelBuilder.Entity<AuditBase>(entity =>
            {
                entity.HasKey(a => a.AuditId);
                entity.Property(a => a.AuditId).UseIdentityColumn();
                entity.Property(a => a.TimeStamp).IsRequired();
                entity.Property(a => a.AuditAction).HasMaxLength(50);
                entity.Property(a => a.UserId).IsRequired().HasMaxLength(450);
                entity.Property(a => a.AuditType) 
                    .HasMaxLength(30)
                    .IsRequired()
                    .HasColumnName("TipoAuditoria");
            });
        }

        /*
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await AuditProductChangesAsync();

            await AuditSupplierChangesAsync();

            //await AuditSystemOperatorChangesAsync();

            await AuditBillChangesAsync();

            await AuditBillDetailChangesAsync();

            await AuditOrderChangesAsync();

            await AuditOrderDetailChangesAsync();

            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task AuditProductChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<Product>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<ProductAudit>();

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;
                var fechaBaja = (DateTime?)entry.CurrentValues["FechaBaja"];

                // Recuperar el UserId del usuario actual
                var user = GetCurrentUser();

                // Crear un registro de auditoría para cada cambio
                var productAudit = new ProductAudit
                {
                    TimeStamp = DateTime.Now,
                    AuditAction = fechaBaja == null ? entry.State.ToString() : EntityState.Deleted.ToString(),
                    UserId = user.Id,
                    ProdId = (string)originalValues["ProdId"]!,
                    Descripcion = (string)originalValues["Descripcion"]!,
                    PrecioUnitario = (decimal)originalValues["PrecioUnitario"]!,
                    Ganancia = (decimal)originalValues["Ganancia"]!,
                    Descuento = (decimal?)originalValues["Descuento"],
                    Stock = (int?)originalValues["Stock"],
                    StockMin = (int?)originalValues["StockMin"],
                    FechaBaja = (DateTime?)originalValues["FechaBaja"]
                };

                audits.Add(productAudit);
            }

            // Añadir el registro de auditoría asincrónicamente
            await Audits.AddRangeAsync(audits);
        }

        public async Task AuditSupplierChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<Supplier>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<SupplierAudit>();

            // Recuperar el UserId del usuario actual
            var user = GetCurrentUser();

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new SupplierAudit
                {
                    TimeStamp = DateTime.Now,
                    AuditAction = entry.State.ToString(),
                    UserId = user.Id,
                    Referencia = (string)originalValues["Referencia"],
                    Contacto = (string?)originalValues["Contacto"],
                    Direccion = (string?)originalValues["Direccion"],
                    Mail = (string?)originalValues["Mail"]
                };

                audits.Add(audit);
            }

            await Audits.AddRangeAsync(audits);
        }

        //public async Task AuditSystemOperatorChangesAsync()
        //{
        //    var entries = ChangeTracker
        //        .Entries<IdentityUser>()
        //        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
        //        .ToList();

        //    var audits = new List<SystemOperatorAudit>();

        //    // Recuperar el UserId del usuario actual
        //    var user = GetCurrentUser();

        //    foreach (var entry in entries)
        //    {
        //        var originalValues = entry.OriginalValues;
        //        var fechaBaja = (DateTime?)entry.CurrentValues["FechaBaja"];

        //        // Obtener el usuario para obtener su rol
        //        var user = GetCurrentUser();

        //        var roles = GetUserRoles(user.Id);

        //        // Crear un registro de auditoría
        //        var audit = new SystemOperatorAudit
        //        {
        //            TimeStamp = DateTime.Now,
        //            AuditAction = entry.State.ToString(),
        //            UserId = user.Id,  // Id de quien hizo la modificación
        //            UserName = (string?)originalValues["UserName"],
        //            Email = (string)originalValues["Email"]!,
        //            PhoneNumber = (string?)originalValues["PhoneNumber"],
        //            FechaBaja = (DateTime?)originalValues["FechaBaja"],
        //            Rol = roles.FirstOrDefault() ?? "Sin rol asignado"
        //        };

        //        audits.Add(audit);
        //    }

        //    await Audits.AddRangeAsync(audits);
        //}

        public async Task AuditBillChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<Bill>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<BillAudit>();

            // Recuperar el UserId del usuario actual
            var user = GetCurrentUser();

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new BillAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = user.Id,
                    FechaHora = (DateTime)originalValues["FechaHora"],
                    Total = (decimal)originalValues["Total"],
                    IdOp = (string)originalValues["IdOp"]
                };

                audits.Add(audit);
            }

            await Audits.AddRangeAsync(audits);
        }

        public async Task AuditOrderChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<Order>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<OrderAudit>();

            // Recuperar el UserId del usuario actual
            var user = GetCurrentUser();

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new OrderAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = user.Id,
                    FechaSolicitud = (DateTime)originalValues["FechaSolicitud"],
                    Estado = (string)originalValues["Estado"],
                    IdOp = (string)originalValues["IdOp"],
                    IdProv = (int)originalValues["IdProv"]
                };

                audits.Add(audit);
            }

            await Audits.AddRangeAsync(audits);
        }

        public async Task AuditBillDetailChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<BillDetail>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<BillDetailAudit>();

            // Recuperar el UserId del usuario actual
            var user = GetCurrentUser();

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new BillDetailAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = user.Id,
                    Cantidad = (int)originalValues["Cantidad"],
                    Precio = (decimal)originalValues["Precio"],
                    Subtotal = (decimal)originalValues["Subtotal"],
                    IdFactura = (int)originalValues["IdFactura"],
                    IdProducto = (string)originalValues["IdProducto"]
                };

                audits.Add(audit);
            }

            await Audits.AddRangeAsync(audits);
        }

        public async Task AuditOrderDetailChangesAsync()
        {
            var entries = ChangeTracker
                .Entries<OrderDetail>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<OrderDetailAudit>();

            // Recuperar el UserId del usuario actual
            var user = GetCurrentUser();

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new OrderDetailAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = user.Id,
                    Cantidad = (int)originalValues["Cantidad"],
                    IdProd = (string)originalValues["IdProducto"],
                    IdOc = (int)originalValues["IdOc"]
                };

                audits.Add(audit);
            }

            await Audits.AddRangeAsync(audits);
        }

        private SystemOperator GetCurrentUser()
        {
            var user = SystemOperators.FirstOrDefault(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
            if (user == null)
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
            }

            return user;
        }
        /*
        private List<string> GetUserRoles(string id)
        {
            return SystemOperators
            .Where(x => x.Id == id)
            .Join(
                Role,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name
            )
            .ToList();
        }
        */
    }
}
