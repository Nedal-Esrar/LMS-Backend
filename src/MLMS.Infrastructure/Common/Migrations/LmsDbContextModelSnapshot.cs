﻿// <auto-generated />
using System;
using MLMS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MLMS.Infrastructure.Common.Migrations
{
    [DbContext(typeof(LmsDbContext))]
    partial class LmsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MLMS.Domain.Common.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("MLMS.Domain.CourseAssignments.CourseAssignment", b =>
                {
                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.Property<int>("MajorId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "MajorId");

                    b.HasIndex("MajorId");

                    b.ToTable("CourseAssignment", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Courses.Course", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<double>("ExpectedTimeToFinishHours")
                        .HasColumnType("float");

                    b.Property<int?>("ExpirationMonths")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Course", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Departments.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Department", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Exams.Choice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Choice", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Exams.Exam", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("int");

                    b.Property<int>("PassThresholdPoints")
                        .HasColumnType("int");

                    b.Property<long>("SectionPartId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Exam", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Exams.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("ExamId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<long>("SectionPartId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.HasIndex("ImageId");

                    b.HasIndex("SectionPartId");

                    b.ToTable("Question", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Files.File", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("File", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Majors.Major", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Major", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Notifications.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("MLMS.Domain.SectionParts.SectionPart", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("ExamId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaterialType")
                        .HasColumnType("int");

                    b.Property<long>("SectionId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExamId")
                        .IsUnique()
                        .HasFilter("[ExamId] IS NOT NULL");

                    b.HasIndex("FileId");

                    b.HasIndex("SectionId");

                    b.ToTable("SectionPart", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Sections.Section", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Section", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.UserSectionParts.UserExamState", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<long>("ExamId")
                        .HasColumnType("bigint");

                    b.Property<long?>("SectionPartId")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ExamId");

                    b.HasIndex("ExamId");

                    b.HasIndex("SectionPartId");

                    b.ToTable("UserExamState", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.UserSectionParts.UserSectionPartDone", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<long>("SectionPartId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDone")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "SectionPartId");

                    b.HasIndex("SectionPartId");

                    b.ToTable("UserSectionPartDone", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.UsersCourses.UserCourse", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("FinishedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCourse", (string)null);
                });

            modelBuilder.Entity("MLMS.Infrastructure.Identity.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("EducationalLevel")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("MajorId")
                        .HasColumnType("int");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ProfilePictureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("WorkId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("MajorId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ProfilePictureId")
                        .IsUnique()
                        .HasFilter("[ProfilePictureId] IS NOT NULL");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MLMS.Domain.Common.Models.RefreshToken", b =>
                {
                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MLMS.Domain.CourseAssignments.CourseAssignment", b =>
                {
                    b.HasOne("MLMS.Domain.Courses.Course", "Course")
                        .WithMany("Assignments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLMS.Domain.Majors.Major", "Major")
                        .WithMany("CourseAssignments")
                        .HasForeignKey("MajorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("MLMS.Domain.Courses.Course", b =>
                {
                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MLMS.Domain.Exams.Choice", b =>
                {
                    b.HasOne("MLMS.Domain.Exams.Question", "Question")
                        .WithMany("Choices")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("MLMS.Domain.Exams.Question", b =>
                {
                    b.HasOne("MLMS.Domain.Exams.Exam", null)
                        .WithMany("Questions")
                        .HasForeignKey("ExamId");

                    b.HasOne("MLMS.Domain.Files.File", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MLMS.Domain.SectionParts.SectionPart", "SectionPart")
                        .WithMany()
                        .HasForeignKey("SectionPartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("SectionPart");
                });

            modelBuilder.Entity("MLMS.Domain.Majors.Major", b =>
                {
                    b.HasOne("MLMS.Domain.Departments.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("MLMS.Domain.Notifications.Notification", b =>
                {
                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MLMS.Domain.SectionParts.SectionPart", b =>
                {
                    b.HasOne("MLMS.Domain.Exams.Exam", "Exam")
                        .WithOne("SectionPart")
                        .HasForeignKey("MLMS.Domain.SectionParts.SectionPart", "ExamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MLMS.Domain.Files.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MLMS.Domain.Sections.Section", "Section")
                        .WithMany("SectionParts")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");

                    b.Navigation("File");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("MLMS.Domain.Sections.Section", b =>
                {
                    b.HasOne("MLMS.Domain.Courses.Course", "Course")
                        .WithMany("Sections")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("MLMS.Domain.UserSectionParts.UserExamState", b =>
                {
                    b.HasOne("MLMS.Domain.Exams.Exam", "Exam")
                        .WithMany()
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLMS.Domain.SectionParts.SectionPart", null)
                        .WithMany("UserExamStates")
                        .HasForeignKey("SectionPartId");

                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("MLMS.Domain.UserSectionParts.UserSectionPartDone", b =>
                {
                    b.HasOne("MLMS.Domain.SectionParts.SectionPart", "SectionPart")
                        .WithMany("UserSectionPartStatuses")
                        .HasForeignKey("SectionPartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SectionPart");
                });

            modelBuilder.Entity("MLMS.Domain.UsersCourses.UserCourse", b =>
                {
                    b.HasOne("MLMS.Domain.Courses.Course", "Course")
                        .WithMany("UsersCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("MLMS.Infrastructure.Identity.Models.ApplicationUser", b =>
                {
                    b.HasOne("MLMS.Domain.Departments.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("MLMS.Domain.Majors.Major", "Major")
                        .WithMany()
                        .HasForeignKey("MajorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MLMS.Domain.Files.File", "ProfilePicture")
                        .WithOne()
                        .HasForeignKey("MLMS.Infrastructure.Identity.Models.ApplicationUser", "ProfilePictureId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Major");

                    b.Navigation("ProfilePicture");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("MLMS.Infrastructure.Identity.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MLMS.Domain.Courses.Course", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Sections");

                    b.Navigation("UsersCourses");
                });

            modelBuilder.Entity("MLMS.Domain.Exams.Exam", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("SectionPart")
                        .IsRequired();
                });

            modelBuilder.Entity("MLMS.Domain.Exams.Question", b =>
                {
                    b.Navigation("Choices");
                });

            modelBuilder.Entity("MLMS.Domain.Majors.Major", b =>
                {
                    b.Navigation("CourseAssignments");
                });

            modelBuilder.Entity("MLMS.Domain.SectionParts.SectionPart", b =>
                {
                    b.Navigation("UserExamStates");

                    b.Navigation("UserSectionPartStatuses");
                });

            modelBuilder.Entity("MLMS.Domain.Sections.Section", b =>
                {
                    b.Navigation("SectionParts");
                });

            modelBuilder.Entity("MLMS.Infrastructure.Identity.Models.ApplicationUser", b =>
                {
                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
