﻿// <auto-generated />
using System;
using CDNAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CDNAPI.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CDNAPI.Models.EntityLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AgoraLog");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("FilePath");

                    b.Property<string>("MinhaCDNLog");

                    b.Property<string>("OutputFormat");

                    b.Property<string>("URL");

                    b.HasKey("Id");

                    b.ToTable("EntityLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
