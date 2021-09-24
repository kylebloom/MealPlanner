﻿// <auto-generated />
using MealPlannerRazor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MealPlannerRazor.Migrations
{
    [DbContext(typeof(MealPlannerRazorContext))]
    [Migration("20210921152943_TransferDatabase")]
    partial class TransferDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MealPlannerRazor.Models.RecipeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Meat")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("RecipeDirections")
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Type")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("RecipeModel");
                });
#pragma warning restore 612, 618
        }
    }
}
