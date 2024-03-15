﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using OrderApp.Data;

#nullable disable

namespace OrderApp.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    partial class OrderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("OrderApp.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("NVARCHAR2(450)")
                        .HasColumnName("Id");

                    b.Property<int>("Amount")
                        .HasColumnType("NUMBER(10)")
                        .HasColumnName("Amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP(7)")
                        .HasColumnName("CreateAt");

                    b.Property<int>("ErrorCode")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)")
                        .HasColumnName("ProductId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)")
                        .HasColumnName("Status");

                    b.HasKey("Id")
                        .HasName("Tbl_OrderId_PK");

                    b.ToTable("Tbl_Order", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
