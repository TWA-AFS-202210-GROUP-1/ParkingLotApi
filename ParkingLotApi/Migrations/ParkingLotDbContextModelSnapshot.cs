﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkingLotApi.Repository;

#nullable disable

namespace ParkingLotApi.Migrations
{
    [DbContext(typeof(ParkingLotDbContext))]
    partial class ParkingLotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ParkingLotApi.Model.OrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CloseTime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CreationTime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NameofParkinglot")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Ordernumber")
                        .HasColumnType("int");

                    b.Property<int?>("ParkingLotEntityId")
                        .HasColumnType("int");

                    b.Property<string>("PlateNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ParkingLotEntityId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ParkingLotApi.Model.ParkingLotEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Parkinglots");
                });

            modelBuilder.Entity("ParkingLotApi.Model.OrderEntity", b =>
                {
                    b.HasOne("ParkingLotApi.Model.ParkingLotEntity", null)
                        .WithMany("Orders")
                        .HasForeignKey("ParkingLotEntityId");
                });

            modelBuilder.Entity("ParkingLotApi.Model.ParkingLotEntity", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
