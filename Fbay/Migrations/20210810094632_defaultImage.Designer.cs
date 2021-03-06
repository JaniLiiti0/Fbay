// <auto-generated />
using Fbay.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fbay.Migrations
{
    [DbContext(typeof(FbayContext))]
    [Migration("20210810094632_defaultImage")]
    partial class defaultImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Fbay.Models.Listings", b =>
                {
                    b.Property<int>("Listing_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Item")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("User_id")
                        .HasColumnType("int");

                    b.HasKey("Listing_id");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("Fbay.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Fname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Lname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
