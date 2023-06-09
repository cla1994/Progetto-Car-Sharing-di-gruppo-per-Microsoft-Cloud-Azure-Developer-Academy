﻿// <auto-generated />
using System;
using Academy2023.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Academy2023.Net.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230420082412_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Academy2023.Net.Models.Car", b =>
                {
                    b.Property<int>("CarID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CarCategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FuelTypeID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxSeat")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RegNum")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("TrunkAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserDataID")
                        .HasColumnType("INTEGER");

                    b.HasKey("CarID");

                    b.HasIndex("CarCategoryID");

                    b.HasIndex("FuelTypeID");

                    b.HasIndex("UserDataID");

                    b.ToTable("cars");
                });

            modelBuilder.Entity("Academy2023.Net.Models.CarCategory", b =>
                {
                    b.Property<int>("CarCategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CarName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CarCategoryID");

                    b.ToTable("CarCategories");
                });

            modelBuilder.Entity("Academy2023.Net.Models.FuelType", b =>
                {
                    b.Property<int>("FuelTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FuelName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FuelTypeID");

                    b.ToTable("FuelTypes");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Gender", b =>
                {
                    b.Property<int>("GenderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GenderName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GenderID");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Picture", b =>
                {
                    b.Property<int>("PictureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PictureName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("RawData")
                        .HasColumnType("BLOB");

                    b.HasKey("PictureID");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Ride", b =>
                {
                    b.Property<int>("RideId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Arrival")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("AvailableSeat")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CarID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Departure")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Km")
                        .HasColumnType("REAL");

                    b.HasKey("RideId");

                    b.HasIndex("CarID");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("Academy2023.Net.Models.UserData", b =>
                {
                    b.Property<int>("UserDataID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthID")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CF")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GenderID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasCar")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("License")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PictureID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RideId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserDataID");

                    b.HasIndex("GenderID");

                    b.HasIndex("PictureID");

                    b.HasIndex("RideId");

                    b.ToTable("usersData");
                });

            modelBuilder.Entity("Academy2023.Net.Models.UserDataRide", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RideId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserDataID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("RideId");

                    b.HasIndex("UserDataID");

                    b.ToTable("userDataRides");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Car", b =>
                {
                    b.HasOne("Academy2023.Net.Models.CarCategory", "CarCategory")
                        .WithMany("Users")
                        .HasForeignKey("CarCategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Academy2023.Net.Models.FuelType", "FuelType")
                        .WithMany("Users")
                        .HasForeignKey("FuelTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Academy2023.Net.Models.UserData", "User")
                        .WithMany("UserCar")
                        .HasForeignKey("UserDataID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarCategory");

                    b.Navigation("FuelType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Ride", b =>
                {
                    b.HasOne("Academy2023.Net.Models.Car", "Car")
                        .WithMany("Rides")
                        .HasForeignKey("CarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("Academy2023.Net.Models.UserData", b =>
                {
                    b.HasOne("Academy2023.Net.Models.Gender", "Gender")
                        .WithMany("Users")
                        .HasForeignKey("GenderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Academy2023.Net.Models.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureID");

                    b.HasOne("Academy2023.Net.Models.Ride", null)
                        .WithMany("Passengers")
                        .HasForeignKey("RideId");

                    b.Navigation("Gender");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("Academy2023.Net.Models.UserDataRide", b =>
                {
                    b.HasOne("Academy2023.Net.Models.Ride", "Ride")
                        .WithMany("UserDataRides")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Academy2023.Net.Models.UserData", "UserData")
                        .WithMany("UserDataRides")
                        .HasForeignKey("UserDataID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ride");

                    b.Navigation("UserData");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Car", b =>
                {
                    b.Navigation("Rides");
                });

            modelBuilder.Entity("Academy2023.Net.Models.CarCategory", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Academy2023.Net.Models.FuelType", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Gender", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Academy2023.Net.Models.Ride", b =>
                {
                    b.Navigation("Passengers");

                    b.Navigation("UserDataRides");
                });

            modelBuilder.Entity("Academy2023.Net.Models.UserData", b =>
                {
                    b.Navigation("UserCar");

                    b.Navigation("UserDataRides");
                });
#pragma warning restore 612, 618
        }
    }
}