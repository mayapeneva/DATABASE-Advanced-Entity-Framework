﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoShare.Data;

namespace PhotoShare.Data.Migrations
{
    [DbContext(typeof(PhotoShareContext))]
    [Migration("20180727121252_Login")]
    partial class Login
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PhotoShare.Models.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BackgroundColor");

                    b.Property<bool>("IsPublic")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("PhotoShare.Models.AlbumRole", b =>
                {
                    b.Property<int>("AlbumId");

                    b.Property<int>("UserId");

                    b.Property<int>("Role");

                    b.HasKey("AlbumId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AlbumRoles");
                });

            modelBuilder.Entity("PhotoShare.Models.AlbumTag", b =>
                {
                    b.Property<int>("AlbumId");

                    b.Property<int>("TagId");

                    b.HasKey("AlbumId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("AlbumTags");
                });

            modelBuilder.Entity("PhotoShare.Models.Friendship", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("FriendId");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("PhotoShare.Models.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlbumId");

                    b.Property<string>("Caption")
                        .HasMaxLength(250);

                    b.Property<string>("Path")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("PhotoShare.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PhotoShare.Models.Town", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasMaxLength(60);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.ToTable("Towns");
                });

            modelBuilder.Entity("PhotoShare.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Age");

                    b.Property<int?>("BornTownId");

                    b.Property<int?>("CurrentTownId");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("FirstName")
                        .HasMaxLength(60)
                        .IsUnicode(true);

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsLogged");

                    b.Property<string>("LastName")
                        .HasMaxLength(60)
                        .IsUnicode(true);

                    b.Property<DateTime?>("LastTimeLoggedIn");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int?>("ProfilePictureId");

                    b.Property<DateTime?>("RegisteredOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("BornTownId");

                    b.HasIndex("CurrentTownId");

                    b.HasIndex("ProfilePictureId")
                        .IsUnique()
                        .HasFilter("[ProfilePictureId] IS NOT NULL");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PhotoShare.Models.AlbumRole", b =>
                {
                    b.HasOne("PhotoShare.Models.Album", "Album")
                        .WithMany("AlbumRoles")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PhotoShare.Models.User", "User")
                        .WithMany("AlbumRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PhotoShare.Models.AlbumTag", b =>
                {
                    b.HasOne("PhotoShare.Models.Album", "Album")
                        .WithMany("AlbumTags")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PhotoShare.Models.Tag", "Tag")
                        .WithMany("AlbumTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PhotoShare.Models.Friendship", b =>
                {
                    b.HasOne("PhotoShare.Models.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PhotoShare.Models.User", "User")
                        .WithMany("FriendsAdded")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PhotoShare.Models.Picture", b =>
                {
                    b.HasOne("PhotoShare.Models.Album", "Album")
                        .WithMany("Pictures")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PhotoShare.Models.User", b =>
                {
                    b.HasOne("PhotoShare.Models.Town", "BornTown")
                        .WithMany("UsersBornInTown")
                        .HasForeignKey("BornTownId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PhotoShare.Models.Town", "CurrentTown")
                        .WithMany("UsersCurrentlyLivingInTown")
                        .HasForeignKey("CurrentTownId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PhotoShare.Models.Picture", "ProfilePicture")
                        .WithOne("UserProfile")
                        .HasForeignKey("PhotoShare.Models.User", "ProfilePictureId");
                });
#pragma warning restore 612, 618
        }
    }
}
