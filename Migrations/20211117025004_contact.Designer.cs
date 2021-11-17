﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PizzaCore.Data;

namespace PizzaCore.Migrations {
  [DbContext(typeof(PizzaCoreContext))]
  [Migration("20211117025004_contact")]
  partial class contact {
    protected override void BuildTargetModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("Relational:MaxIdentifierLength", 128)
          .HasAnnotation("ProductVersion", "5.0.12")
          .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

      modelBuilder.Entity("PizzaCore.Models.ContactModel", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

        b.Property<DateTime>("Date")
            .HasColumnType("datetime2");

        b.Property<string>("Email")
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnType("nvarchar(30)");

        b.Property<string>("FirstName")
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        b.Property<string>("LastName")
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        b.Property<string>("Message")
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("nvarchar(500)");

        b.Property<string>("Topic")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.HasKey("Id");

        b.ToTable("ContactModel");
      });
#pragma warning restore 612, 618
    }
  }
}