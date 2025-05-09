﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orari.DataDbContext;


#nullable disable

namespace Orari.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250329182057_InitialCreation")]
    partial class InitialCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Orari.Models.Courses", b =>
                {
                    b.Property<int>("CId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CId"));

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PId")
                        .HasColumnType("int");

                    b.Property<string>("Professor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Orari.Models.Enrollments", b =>
                {
                    b.Property<int>("EId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EId"));

                    b.Property<int>("CId")
                        .HasColumnType("int");

                    b.Property<int>("SId")
                        .HasColumnType("int");

                    b.HasKey("EId");

                    b.HasIndex("CId");

                    b.HasIndex("SId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("Orari.Models.Exams", b =>
                {
                    b.Property<int>("EId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EId"));

                    b.Property<int>("CId")
                        .HasColumnType("int");

                    b.Property<int>("CourseCId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("ExamDate")
                        .HasColumnType("date");

                    b.Property<string>("ExamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SCId")
                        .HasColumnType("int");

                    b.HasKey("EId");

                    b.HasIndex("CourseCId");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("Orari.Models.Profesors", b =>
                {
                    b.Property<int>("PId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PId"));

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<string>("PName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpecialRequirements")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PId");

                    b.ToTable("Professors");
                });

            modelBuilder.Entity("Orari.Models.Rooms", b =>
                {
                    b.Property<int>("RId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RId"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("RName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Orari.Models.Schedules", b =>
                {
                    b.Property<int>("SCId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SCId"));

                    b.Property<int>("CId")
                        .HasColumnType("int");

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("EId")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<int>("PId")
                        .HasColumnType("int");

                    b.Property<string>("Profesor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RId")
                        .HasColumnType("int");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("SCId");

                    b.HasIndex("EId")
                        .IsUnique();

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Orari.Models.Students", b =>
                {
                    b.Property<int>("SId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Orari.Models.Enrollments", b =>
                {
                    b.HasOne("Orari.Models.Courses", "Courses")
                        .WithMany("Enrollments")
                        .HasForeignKey("CId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Orari.Models.Students", "Students")
                        .WithMany("Enrollments")
                        .HasForeignKey("SId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Courses");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Orari.Models.Exams", b =>
                {
                    b.HasOne("Orari.Models.Courses", "Course")
                        .WithMany()
                        .HasForeignKey("CourseCId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Orari.Models.Schedules", b =>
                {
                    b.HasOne("Orari.Models.Exams", "Exam")
                        .WithOne("Schedule")
                        .HasForeignKey("Orari.Models.Schedules", "EId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("Orari.Models.Courses", b =>
                {
                    b.Navigation("Enrollments");
                });

            modelBuilder.Entity("Orari.Models.Exams", b =>
                {
                    b.Navigation("Schedule")
                        .IsRequired();
                });

            modelBuilder.Entity("Orari.Models.Students", b =>
                {
                    b.Navigation("Enrollments");
                });
#pragma warning restore 612, 618
        }
    }
}
