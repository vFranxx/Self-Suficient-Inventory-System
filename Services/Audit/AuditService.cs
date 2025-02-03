using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Self_Suficient_Inventory_System.Models.AuditModels;
using RESTful_API.Models.Entities;
using RESTful_API.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Self_Suficient_Inventory_System.Services
{
    public class AuditService
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public AuditService(AppDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            IHttpContextAccessor httpContextAccessor,
                            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }

        public async Task AuditProductChangesAsync()
        {
            var entries = _dbContext.ChangeTracker
                .Entries<Product>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<ProductAudit>();

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;
                var fechaBaja = (DateTime?)entry.CurrentValues["FechaBaja"];

                // Recuperar el UserId del usuario actual
                var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
                }

                // Crear un registro de auditoría para cada cambio
                var productAudit = new ProductAudit
                {
                    TimeStamp = DateTime.Now,
                    AuditAction = fechaBaja == null ? entry.State.ToString() : EntityState.Deleted.ToString(),
                    UserId = userId,
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
            await _dbContext.Audits.AddRangeAsync(audits);
        }

        public async Task AuditSupplierChangesAsync()
        {
            var entries = _dbContext.ChangeTracker
                .Entries<Supplier>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<SupplierAudit>();

            // Recuperar el UserId del usuario actual
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
            }

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new SupplierAudit
                {
                    TimeStamp = DateTime.Now,
                    AuditAction = entry.State.ToString(),
                    UserId = userId,
                    Referencia = (string)originalValues["Referencia"],
                    Contacto = (string?)originalValues["Contacto"],
                    Direccion = (string?)originalValues["Direccion"],
                    Mail = (string?)originalValues["Mail"]
                };

                audits.Add(audit);
            }

            await _dbContext.Audits.AddRangeAsync(audits);
        }

        public async Task AuditSystemOperatorChangesAsync()
        {
            var entries = _dbContext.ChangeTracker
                .Entries<IdentityUser>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<SystemOperatorAudit>();

            // Recuperar el UserId del usuario actual
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
            }

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;
                var fechaBaja = (DateTime?)entry.CurrentValues["FechaBaja"];

                // Obtener el usuario para obtener su rol
                var systemOperator = await _userManager.FindByIdAsync(userId);
                if (systemOperator == null)
                {
                    throw new InvalidOperationException("Usuario no encontrado para auditoría.");
                }

                var roles = await _userManager.GetRolesAsync(systemOperator);
                var role = roles.FirstOrDefault();

                // Crear un registro de auditoría
                var audit = new SystemOperatorAudit
                {
                    TimeStamp = DateTime.Now,
                    AuditAction = entry.State.ToString(),
                    UserId = userId,  // Id de quien hizo la modificación
                    UserName = (string?)originalValues["UserName"],
                    Email = (string)originalValues["Email"]!,
                    PhoneNumber = (string?)originalValues["PhoneNumber"],
                    FechaBaja = (DateTime?)originalValues["FechaBaja"],
                    Rol = role!
                };

                audits.Add(audit);
            }

            await _dbContext.Audits.AddRangeAsync(audits);
        }

        public async Task AuditBillChangesAsync()
        {
            var entries = _dbContext.ChangeTracker
                .Entries<Bill>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<BillAudit>();

            // Recuperar el UserId del usuario actual
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
            }

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new BillAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = userId,
                    FechaHora = (DateTime)originalValues["FechaHora"],
                    Total = (decimal)originalValues["Total"],
                    IdOp = (string)originalValues["IdOp"]
                };

                audits.Add(audit);
            }

            await _dbContext.Audits.AddRangeAsync(audits);
        }

        public async Task AuditOrderChangesAsync()
        {
            var entries = _dbContext.ChangeTracker
                .Entries<Order>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<OrderAudit>();

            // Recuperar el UserId del usuario actual
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
            }

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new OrderAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = userId,
                    FechaSolicitud = (DateTime)originalValues["FechaSolicitud"],
                    Estado = (string)originalValues["Estado"],
                    IdOp = (string)originalValues["IdOp"],
                    IdProv = (int)originalValues["IdProv"]
                };

                audits.Add(audit);
            }

            await _dbContext.Audits.AddRangeAsync(audits);
        }

        public async Task AuditBillDetailChangesAsync()
        {
            var entries = _dbContext.ChangeTracker
                .Entries<BillDetail>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<BillDetailAudit>();

            // Recuperar el UserId del usuario actual
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
            }

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new BillDetailAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = userId,
                    Cantidad = (int)originalValues["Cantidad"],
                    Precio = (decimal)originalValues["Precio"],
                    Subtotal = (decimal)originalValues["Subtotal"],
                    IdFactura = (int)originalValues["IdFactura"],
                    IdProducto = (string)originalValues["IdProducto"]
                };

                audits.Add(audit);
            }

            await _dbContext.Audits.AddRangeAsync(audits);
        }

        public async Task AuditOrderDetailChangesAsync()
        {
            var entries = _dbContext.ChangeTracker
                .Entries<OrderDetail>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var audits = new List<OrderDetailAudit>();

            // Recuperar el UserId del usuario actual
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("No se pudo obtener el ID del usuario autenticado.");
            }

            foreach (var entry in entries)
            {
                var originalValues = entry.OriginalValues;

                // Crear registro de auditoria
                var audit = new OrderDetailAudit
                {
                    TimeStamp = DateTime.UtcNow,
                    AuditAction = entry.State.ToString(),
                    UserId = userId,
                    Cantidad = (int)originalValues["Cantidad"],
                    IdProd = (string)originalValues["IdProducto"],
                    IdOc = (int)originalValues["IdOc"]
                };

                audits.Add(audit);
            }

            await _dbContext.Audits.AddRangeAsync(audits);
        }
    }
}