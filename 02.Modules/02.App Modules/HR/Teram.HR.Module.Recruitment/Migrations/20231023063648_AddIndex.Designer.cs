﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Teram.HR.Module.Recruitment.Entities.DbContext;

#nullable disable

namespace Teram.HR.Module.Recruitment.Migrations
{
    [DbContext(typeof(HRContext))]
    [Migration("20231023063648_AddIndex")]
    partial class AddIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Teram.HR.Module.Recruitment.Entities.JobApplicants", b =>
                {
                    b.Property<int>("JobApplicantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobApplicantId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BirthCertificateNo")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("BirthCertificateSerial")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Citizenship")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("GenderType")
                        .HasColumnType("int");

                    b.Property<decimal?>("Latitude")
                        .HasPrecision(18, 15)
                        .HasColumnType("decimal(18,15)");

                    b.Property<decimal?>("Longitude")
                        .HasPrecision(18, 15)
                        .HasColumnType("decimal(18,15)");

                    b.Property<int>("MajorId")
                        .HasColumnType("int");

                    b.Property<int>("MaritalStatus")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NationalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Religion")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("JobApplicantId");

                    b.HasIndex("MajorId");

                    b.HasIndex("NationalCode", "Email")
                        .IsUnique();

                    b.ToTable("JobApplicants", "HR");
                });

            modelBuilder.Entity("Teram.HR.Module.Recruitment.Entities.Major", b =>
                {
                    b.Property<int>("MajorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MajorId"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MajorId");

                    b.ToTable("Majors", "HR");
                });

            modelBuilder.Entity("Teram.HR.Module.Recruitment.Entities.JobApplicants", b =>
                {
                    b.HasOne("Teram.HR.Module.Recruitment.Entities.Major", "Major")
                        .WithMany("JobApplicants")
                        .HasForeignKey("MajorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Major");
                });

            modelBuilder.Entity("Teram.HR.Module.Recruitment.Entities.Major", b =>
                {
                    b.Navigation("JobApplicants");
                });
#pragma warning restore 612, 618
        }
    }
}