// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PermAdminAPI.Data;

#nullable disable

namespace PermAdminAPI.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250401000000_AddAssignedOnField")]
    partial class AddAssignedOnField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("PermAdminAPI.Models.AppUser", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PermAdminAPI.Models.Employee", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("PermAdminAPI.Models.EmployeeLicence", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AssignedOn")
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("employeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("licenceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("employeeId");

                    b.HasIndex("licenceId");

                    b.ToTable("EmployeeLicences");
                });

            modelBuilder.Entity("PermAdminAPI.Models.Licence", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Licences");
                });

            modelBuilder.Entity("PermAdminAPI.Models.LicenceInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LicenceId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LicenceId");

                    b.ToTable("LicenceInstances");
                });

            modelBuilder.Entity("PermAdminAPI.Models.EmployeeLicence", b =>
                {
                    b.HasOne("PermAdminAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("employeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PermAdminAPI.Models.Licence", "Licence")
                        .WithMany()
                        .HasForeignKey("licenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Licence");
                });
#pragma warning restore 612, 618
        }
    }
}
