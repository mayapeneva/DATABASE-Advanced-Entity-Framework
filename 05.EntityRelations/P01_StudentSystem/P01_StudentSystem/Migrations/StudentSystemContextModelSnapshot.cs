﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using P01_StudentSystem.Data;
using P01_StudentSystem.Enums;
using System;

namespace P01_StudentSystem.Migrations
{
    [DbContext(typeof(StudentSystemContext))]
    partial class StudentSystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasColumnName("Description")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("NVARCHAR(80)");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Homework", b =>
                {
                    b.Property<int>("HomeworkId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnName("Content")
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<int>("ContentType");

                    b.Property<int>("CourseId");

                    b.Property<int>("StudentId");

                    b.Property<DateTime>("SubmissionTime");

                    b.HasKey("HomeworkId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("HomeworkSubmissions");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Resource", b =>
                {
                    b.Property<int>("ResourceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<int>("ResourceType");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnName("Url")
                        .HasColumnType("VARCHAR(MAX)");

                    b.HasKey("ResourceId");

                    b.HasIndex("CourseId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Birthday");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("NVARCHAR(100)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("PhoneNumber")
                        .HasColumnType("VARCHAR(10)")
                        .HasMaxLength(10);

                    b.Property<DateTime>("RegisteredOn");

                    b.HasKey("StudentId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.StudentCourse", b =>
                {
                    b.Property<int>("StudentId");

                    b.Property<int>("CourseId");

                    b.HasKey("StudentId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("StudentCourses");
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Homework", b =>
                {
                    b.HasOne("P01_StudentSystem.Data.Models.Course", "Course")
                        .WithMany("HomeworkSubmissions")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("P01_StudentSystem.Data.Models.Student", "Student")
                        .WithMany("HomeworkSubmissions")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.Resource", b =>
                {
                    b.HasOne("P01_StudentSystem.Data.Models.Course", "Course")
                        .WithMany("Resources")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("P01_StudentSystem.Data.Models.StudentCourse", b =>
                {
                    b.HasOne("P01_StudentSystem.Data.Models.Course", "Course")
                        .WithMany("StudentsEnrolled")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("P01_StudentSystem.Data.Models.Student", "Student")
                        .WithMany("CourseEnrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
