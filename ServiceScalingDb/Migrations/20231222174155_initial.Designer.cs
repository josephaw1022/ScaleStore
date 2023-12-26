﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceScalingDb.ScalingDb;

#nullable disable

namespace ServiceScalingDb.Migrations
{
    [DbContext(typeof(ScalingDbContext))]
    [Migration("20231222174155_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Application", b =>
                {
                    b.Property<int>("ApplicationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ApplicationID"));

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProjectID")
                        .HasColumnType("integer");

                    b.HasKey("ApplicationID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Environment", b =>
                {
                    b.Property<int>("EnvironmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EnvironmentID"));

                    b.Property<string>("EnvironmentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProjectID")
                        .HasColumnType("integer");

                    b.HasKey("EnvironmentID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProjectID"));

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProjectID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.ScalingConfiguration", b =>
                {
                    b.Property<int>("ScalingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ScalingID"));

                    b.Property<int>("ApplicationID")
                        .HasColumnType("integer");

                    b.Property<int>("EnvironmentID")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfInstances")
                        .HasColumnType("integer");

                    b.HasKey("ScalingID");

                    b.HasIndex("ApplicationID");

                    b.HasIndex("EnvironmentID");

                    b.ToTable("ScalingConfigurations");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Application", b =>
                {
                    b.HasOne("ServiceScalingDb.ScalingDb.Project", "Project")
                        .WithMany("Applications")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Environment", b =>
                {
                    b.HasOne("ServiceScalingDb.ScalingDb.Project", "Project")
                        .WithMany("Environments")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.ScalingConfiguration", b =>
                {
                    b.HasOne("ServiceScalingDb.ScalingDb.Application", "Application")
                        .WithMany("ScalingConfigurations")
                        .HasForeignKey("ApplicationID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ServiceScalingDb.ScalingDb.Environment", "Environment")
                        .WithMany("ScalingConfigurations")
                        .HasForeignKey("EnvironmentID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Environment");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Application", b =>
                {
                    b.Navigation("ScalingConfigurations");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Environment", b =>
                {
                    b.Navigation("ScalingConfigurations");
                });

            modelBuilder.Entity("ServiceScalingDb.ScalingDb.Project", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Environments");
                });
#pragma warning restore 612, 618
        }
    }
}
