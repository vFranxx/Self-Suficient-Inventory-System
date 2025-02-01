﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RESTful_API.Data;

#nullable disable

namespace RESTful_API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241220163313_New Migration 1")]
    partial class NewMigration1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RESTful_API.Models.Entities.Bill", b =>
                {
                    b.Property<int>("FacId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacId"));

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdOp")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("FacId");

                    b.HasIndex("IdOp");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.BillDetail", b =>
                {
                    b.Property<int>("FacDetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacDetId"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("IdFactura")
                        .HasColumnType("int");

                    b.Property<string>("IdProducto")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(12,2)");

                    b.Property<decimal>("Subtotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("FacDetId");

                    b.HasIndex("IdFactura");

                    b.HasIndex("IdProducto");

                    b.ToTable("BillDetails");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.Order", b =>
                {
                    b.Property<int>("OcId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OcId"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("FechaSolicitud")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdOp")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IdProv")
                        .HasColumnType("int");

                    b.HasKey("OcId");

                    b.HasIndex("IdOp");

                    b.HasIndex("IdProv");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.OrderDetail", b =>
                {
                    b.Property<int>("DetOcId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DetOcId"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("IdOc")
                        .HasColumnType("int");

                    b.Property<string>("IdProd")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("DetOcId");

                    b.HasIndex("IdOc");

                    b.HasIndex("IdProd");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.Product", b =>
                {
                    b.Property<string>("ProdId")
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Ganancia")
                        .HasColumnType("decimal(5,2)");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(12,2)");

                    b.Property<int?>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProdId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.Supplier", b =>
                {
                    b.Property<int>("ProvId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProvId"));

                    b.Property<string>("Contacto")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Direccion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Mail")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Referencia")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("ProvId");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.SupplierProduct", b =>
                {
                    b.Property<int>("IdProv")
                        .HasColumnType("int");

                    b.Property<string>("IdProd")
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("IdProv", "IdProd");

                    b.HasIndex("IdProd");

                    b.ToTable("SupplierProducts");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.SystemOperator", b =>
                {
                    b.Property<string>("Uid")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Pswd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Tipo")
                        .HasColumnType("bit");

                    b.HasKey("Uid");

                    b.ToTable("SystemOperators");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.Bill", b =>
                {
                    b.HasOne("RESTful_API.Models.Entities.SystemOperator", "Operators")
                        .WithMany()
                        .HasForeignKey("IdOp")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Operators");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.BillDetail", b =>
                {
                    b.HasOne("RESTful_API.Models.Entities.Bill", "Facturas")
                        .WithMany()
                        .HasForeignKey("IdFactura")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RESTful_API.Models.Entities.Product", "Productos")
                        .WithMany()
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Facturas");

                    b.Navigation("Productos");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.Order", b =>
                {
                    b.HasOne("RESTful_API.Models.Entities.SystemOperator", "Operators")
                        .WithMany()
                        .HasForeignKey("IdOp")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RESTful_API.Models.Entities.Supplier", "Suppliers")
                        .WithMany()
                        .HasForeignKey("IdProv")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Operators");

                    b.Navigation("Suppliers");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.OrderDetail", b =>
                {
                    b.HasOne("RESTful_API.Models.Entities.Order", "Orders")
                        .WithMany()
                        .HasForeignKey("IdOc")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RESTful_API.Models.Entities.Product", "Products")
                        .WithMany()
                        .HasForeignKey("IdProd")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Orders");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("RESTful_API.Models.Entities.SupplierProduct", b =>
                {
                    b.HasOne("RESTful_API.Models.Entities.Product", "Products")
                        .WithMany()
                        .HasForeignKey("IdProd")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RESTful_API.Models.Entities.Supplier", "Suppliers")
                        .WithMany()
                        .HasForeignKey("IdProv")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Products");

                    b.Navigation("Suppliers");
                });
#pragma warning restore 612, 618
        }
    }
}
