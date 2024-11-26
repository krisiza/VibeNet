﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VibeNet.Infrastucture.Data;

#nullable disable

namespace VibeNet.Infrastucture.Migrations
{
    [DbContext(typeof(VibeNetDbContext))]
    partial class VibeNetDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("VibeNet.Infrastucture.Data.Models.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Like Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User Owner Identifier");

                    b.Property<int?>("PostId")
                        .HasColumnType("int")
                        .HasComment("Post Like Identifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.ToTable("Likes", (string)null);
                });

            modelBuilder.Entity("VibeNet.Infrastucture.Data.Models.ProfilePicture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Picture Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Picture Type");

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)")
                        .HasComment("Picture Data");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Picture Name");

                    b.HasKey("Id");

                    b.ToTable("ProfilePictures", null, t =>
                        {
                            t.HasComment("User Profil Picture");
                        });
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Comment Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasComment("Comment Content");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("Comment Is Active Or Not");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User Owner Identifier");

                    b.Property<int?>("PostId")
                        .HasColumnType("int")
                        .HasComment("Post Comment Identifier");

                    b.Property<DateTime>("PostedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Comment Creation Date");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments", null, t =>
                        {
                            t.HasComment("Post Comment From User");
                        });
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Friendship", b =>
                {
                    b.Property<string>("FirstUserId")
                        .HasColumnType("nvarchar(450)")
                        .HasComment("First User Identifier");

                    b.Property<string>("SecondUserId")
                        .HasColumnType("nvarchar(450)")
                        .HasComment("Second User Identifier");

                    b.Property<DateTime>("FriendsSince")
                        .HasColumnType("datetime2")
                        .HasComment("Friendship Since Date");

                    b.HasKey("FirstUserId", "SecondUserId");

                    b.HasIndex("SecondUserId");

                    b.ToTable("Friendships", null, t =>
                        {
                            t.HasComment("Friendship between Users");
                        });
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Friendshiprequest", b =>
                {
                    b.Property<string>("UserRecipientId")
                        .HasColumnType("nvarchar(450)")
                        .HasComment("Recipient Identifier");

                    b.Property<string>("UserTransmitterId")
                        .HasColumnType("nvarchar(450)")
                        .HasComment("Transmitter Identifier");

                    b.HasKey("UserRecipientId", "UserTransmitterId");

                    b.HasIndex("UserTransmitterId");

                    b.ToTable("Friendshiprequests", null, t =>
                        {
                            t.HasComment("Friendshiprequest");
                        });
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Post Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)")
                        .HasComment("Post Content");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("Post Is Active Or Not");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("Post Owner Identifier");

                    b.Property<int?>("Picture")
                        .HasColumnType("int")
                        .HasComment("Post Picture");

                    b.Property<DateTime>("PostedOn")
                        .HasColumnType("datetime2")
                        .HasComment("Post Creation Date");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Posts", null, t =>
                        {
                            t.HasComment("User Post");
                        });
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.VibeNetUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("User Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2")
                        .HasComment("User Birthday");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2")
                        .HasComment("User Profile Creation Date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("User Firstname");

                    b.Property<int?>("Gender")
                        .HasColumnType("int")
                        .HasComment("User Gender");

                    b.Property<string>("HomeTown")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("User HomeTown");

                    b.Property<string>("IdentityUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("Identity User Identifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit")
                        .HasComment("User Profil Activated Or Not");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("User Secondname");

                    b.Property<int>("ProfilePictureId")
                        .HasColumnType("int")
                        .HasComment("User ProfilPicture Identifier");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.HasIndex("ProfilePictureId");

                    b.ToTable("VibeNetUsers", null, t =>
                        {
                            t.HasComment("Application User");
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VibeNet.Infrastucture.Data.Models.Like", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VibeNetInfrastucture.Data.Models.Post", "Post")
                        .WithMany("UserLiked")
                        .HasForeignKey("PostId");

                    b.Navigation("Owner");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Comment", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VibeNetInfrastucture.Data.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");

                    b.Navigation("Owner");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Friendship", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "FirstUser")
                        .WithMany()
                        .HasForeignKey("FirstUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "SecondUser")
                        .WithMany()
                        .HasForeignKey("SecondUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FirstUser");

                    b.Navigation("SecondUser");
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Friendshiprequest", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "UserRecipient")
                        .WithMany()
                        .HasForeignKey("UserRecipientId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "UserTransmitter")
                        .WithMany()
                        .HasForeignKey("UserTransmitterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("UserRecipient");

                    b.Navigation("UserTransmitter");
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Post", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.VibeNetUser", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("IdentityUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VibeNet.Infrastucture.Data.Models.ProfilePicture", "ProfilePicture")
                        .WithMany()
                        .HasForeignKey("ProfilePictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProfilePicture");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VibeNetInfrastucture.Data.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("UserLiked");
                });
#pragma warning restore 612, 618
        }
    }
}
