﻿// <auto-generated />
using System;
using Baza.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Baza.Migrations
{
    [DbContext(typeof(LinkAggregator))]
    partial class LinkAggregatorModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Baza.Models.Likes", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("LinkID");

                    b.HasKey("UserID");

                    b.HasIndex("LinkID");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Baza.Models.Links", b =>
                {
                    b.Property<int>("LinkId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Link");

                    b.Property<string>("Name");

                    b.Property<int>("UserId");

                    b.HasKey("LinkId");

                    b.HasIndex("UserId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Baza.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Baza.Models.Likes", b =>
                {
                    b.HasOne("Baza.Models.Links", "Links")
                        .WithMany("Likes")
                        .HasForeignKey("LinkID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Baza.Models.Users", "Users")
                        .WithMany("Likes")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Baza.Models.Links", b =>
                {
                    b.HasOne("Baza.Models.Users", "Users")
                        .WithMany("Links")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
