﻿// <auto-generated />
using System;
using BotEventManagement.Services.Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BotEventManagement.Services.Migrations
{
    [DbContext(typeof(BotEventManagementContext))]
    [Migration("20190319212651_ChangeComplexType")]
    partial class ChangeComplexType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("BotEventManagement")
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BotEventManagement.Models.Database.Activity", b =>
                {
                    b.Property<string>("ActivityId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("EventId");

                    b.Property<string>("Name");

                    b.Property<string>("SpeakerId");

                    b.HasKey("ActivityId");

                    b.HasIndex("EventId");

                    b.HasIndex("SpeakerId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.Event", b =>
                {
                    b.Property<string>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("EventId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.EventParticipants", b =>
                {
                    b.Property<string>("GuestId");

                    b.Property<string>("EventId");

                    b.Property<string>("Name");

                    b.HasKey("GuestId", "EventId");

                    b.HasIndex("EventId");

                    b.HasIndex("GuestId", "EventId");

                    b.ToTable("EventParticipants");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.GuestUserTalks", b =>
                {
                    b.Property<string>("GuestId");

                    b.Property<string>("ActivityId");

                    b.Property<string>("EventId");

                    b.HasKey("GuestId", "ActivityId");

                    b.HasIndex("ActivityId");

                    b.HasIndex("EventId");

                    b.HasIndex("GuestId", "ActivityId");

                    b.ToTable("UserTalks");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.Speaker", b =>
                {
                    b.Property<string>("SpeakerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Biography");

                    b.Property<string>("EventId");

                    b.Property<string>("Name");

                    b.Property<string>("UploadedPhoto");

                    b.HasKey("SpeakerId");

                    b.HasIndex("EventId");

                    b.ToTable("Speaker");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.User", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.UserEvents", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("EventId");

                    b.HasKey("UserId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("UserEvents");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.Activity", b =>
                {
                    b.HasOne("BotEventManagement.Models.Database.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.HasOne("BotEventManagement.Models.Database.Speaker", "Speaker")
                        .WithMany("Activity")
                        .HasForeignKey("SpeakerId");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.Event", b =>
                {
                    b.OwnsOne("BotEventManagement.Models.Database.Address", "Address", b1 =>
                        {
                            b1.Property<string>("EventId");

                            b1.Property<double>("Latitude");

                            b1.Property<double>("Longitude");

                            b1.Property<string>("Street");

                            b1.HasKey("EventId");

                            b1.ToTable("Event");

                            b1.HasOne("BotEventManagement.Models.Database.Event")
                                .WithOne("Address")
                                .HasForeignKey("BotEventManagement.Models.Database.Address", "EventId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.EventParticipants", b =>
                {
                    b.HasOne("BotEventManagement.Models.Database.Event", "Event")
                        .WithMany("EventParticipants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.GuestUserTalks", b =>
                {
                    b.HasOne("BotEventManagement.Models.Database.Activity", "Activity")
                        .WithMany("UserTalks")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BotEventManagement.Models.Database.Event")
                        .WithMany("UserTalks")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.Speaker", b =>
                {
                    b.HasOne("BotEventManagement.Models.Database.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("BotEventManagement.Models.Database.UserEvents", b =>
                {
                    b.HasOne("BotEventManagement.Models.Database.Event", "Event")
                        .WithMany("UserEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BotEventManagement.Models.Database.User", "User")
                        .WithMany("UserEvents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
