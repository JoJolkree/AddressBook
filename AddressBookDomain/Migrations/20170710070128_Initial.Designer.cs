﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AddressBookDomain.Domain;

namespace AddressBookDomain.Migrations
{
    [DbContext(typeof(AddressBookContext))]
    [Migration("20170710070128_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AddressBookDomain.Domain.Call", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContactId");

                    b.Property<DateTime>("Created");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Calls");
                });

            modelBuilder.Entity("AddressBookDomain.Domain.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Note");

                    b.Property<string>("PhoneNumber");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("AddressBookDomain.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Login");

                    b.Property<string>("Password");

                    b.Property<int>("UserType");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AddressBookDomain.Domain.Call", b =>
                {
                    b.HasOne("AddressBookDomain.Domain.Contact", "Contact")
                        .WithMany("Calls")
                        .HasForeignKey("ContactId");
                });

            modelBuilder.Entity("AddressBookDomain.Domain.Contact", b =>
                {
                    b.HasOne("AddressBookDomain.Domain.User", "User")
                        .WithMany("Contacts")
                        .HasForeignKey("UserId");
                });
        }
    }
}
