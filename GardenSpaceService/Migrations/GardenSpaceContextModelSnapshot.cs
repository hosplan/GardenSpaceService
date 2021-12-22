﻿// <auto-generated />
using System;
using GardenSpaceService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GardenSpaceService.Migrations
{
    [DbContext(typeof(GardenSpaceContext))]
    partial class GardenSpaceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GardenSpaceService.Model.GardenBranchType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RootTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RootTypeId");

                    b.ToTable("GardenBranchType");
                });

            modelBuilder.Entity("GardenSpaceService.Model.GardenParticipateRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GardenBranchId")
                        .HasColumnType("int");

                    b.Property<int>("GardenSpaceId")
                        .HasColumnType("int");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GardenBranchId");

                    b.HasIndex("GardenSpaceId");

                    b.ToTable("GardenParticipateRole");
                });

            modelBuilder.Entity("GardenSpaceService.Model.GardenRootType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GardenRootType");
                });

            modelBuilder.Entity("GardenSpaceService.Model.GardenSpace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BranchId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<bool>("OnlyInvite")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("PlanEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PlanStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SpaceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("GardenSpace");
                });

            modelBuilder.Entity("GardenSpaceService.Model.GardenSpaceUserMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GardenSpaceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ParticiDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GardenSpaceId");

                    b.ToTable("GardenSpaceUserMap");
                });

            modelBuilder.Entity("GardenSpaceService.Model.GardenBranchType", b =>
                {
                    b.HasOne("GardenSpaceService.Model.GardenRootType", "RootType")
                        .WithMany()
                        .HasForeignKey("RootTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RootType");
                });

            modelBuilder.Entity("GardenSpaceService.Model.GardenParticipateRole", b =>
                {
                    b.HasOne("GardenSpaceService.Model.GardenBranchType", "GardenBranchType")
                        .WithMany()
                        .HasForeignKey("GardenBranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GardenSpaceService.Model.GardenSpace", "GardenSpace")
                        .WithMany()
                        .HasForeignKey("GardenSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GardenBranchType");

                    b.Navigation("GardenSpace");
                });

            modelBuilder.Entity("GardenSpaceService.Model.GardenSpaceUserMap", b =>
                {
                    b.HasOne("GardenSpaceService.Model.GardenSpace", "GardenSpace")
                        .WithMany()
                        .HasForeignKey("GardenSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GardenSpace");
                });
#pragma warning restore 612, 618
        }
    }
}
