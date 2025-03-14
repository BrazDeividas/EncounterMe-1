﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPI.Database;

namespace WebAPI.Migrations
{
    [DbContext(typeof(BaseDbContext))]
    [Migration("20220405162511_user-seed")]
    partial class userseed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.15");

            modelBuilder.Entity("EncounterMe.Classes.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LocationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Attribute");
                });

            modelBuilder.Entity("EncounterMe.Friend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FriendId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("EncounterMe.FriendRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeSent")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("EncounterMe.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<float>("Longtitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("EncounterMe.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AchievementNum")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FoundLocationNum")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Hashpassword")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("LevelPoints")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessLevel = 0,
                            AchievementNum = 10,
                            Email = "mrhamster@gmail.com",
                            FoundLocationNum = 23,
                            Hashpassword = new byte[] { 21, 38, 44, 23, 2, 171, 15, 221, 119, 242, 49, 206, 209, 1, 142, 230, 198, 205, 247, 107, 74, 54, 221, 49, 170, 135, 107, 6, 239, 179, 213, 51 },
                            LevelPoints = 8520,
                            Name = "Hamster",
                            Password = "ilovehamsters"
                        },
                        new
                        {
                            Id = 2,
                            AccessLevel = 0,
                            AchievementNum = 10,
                            Email = "mrcamster@gmail.com",
                            FoundLocationNum = 23,
                            Hashpassword = new byte[] { 21, 38, 44, 23, 2, 171, 15, 221, 119, 242, 49, 206, 209, 1, 142, 230, 198, 205, 247, 107, 74, 54, 221, 49, 170, 135, 107, 6, 239, 179, 213, 51 },
                            LevelPoints = 8520,
                            Name = "Camster",
                            Password = "ilovecamsters"
                        });
                });

            modelBuilder.Entity("EncounterMe.Classes.Attribute", b =>
                {
                    b.HasOne("EncounterMe.Location", null)
                        .WithMany("Attributes")
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("EncounterMe.Friend", b =>
                {
                    b.HasOne("EncounterMe.User", "User")
                        .WithMany("Friends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EncounterMe.FriendRequest", b =>
                {
                    b.HasOne("EncounterMe.User", "User")
                        .WithMany("FriendRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EncounterMe.Location", b =>
                {
                    b.Navigation("Attributes");
                });

            modelBuilder.Entity("EncounterMe.User", b =>
                {
                    b.Navigation("FriendRequests");

                    b.Navigation("Friends");
                });
#pragma warning restore 612, 618
        }
    }
}
