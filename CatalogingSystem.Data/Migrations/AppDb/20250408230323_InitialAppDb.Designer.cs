﻿// <auto-generated />
using System;
using CatalogingSystem.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CatalogingSystem.Data.Migrations.AppDb
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250408230323_InitialAppDb")]
    partial class InitialAppDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CatalogingSystem.Core.Entities.ArchivoAdministrativo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("archivoDocumental")
                        .HasColumnType("text");

                    b.Property<string>("asunto")
                        .HasColumnType("text");

                    b.Property<string>("documentoOrigen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("expediente")
                        .HasColumnType("bigint");

                    b.Property<string>("expedienteAnterior")
                        .HasColumnType("text");

                    b.Property<DateTime?>("fechaFinal")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("fechaInicial")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("historial")
                        .HasColumnType("text");

                    b.Property<string>("institucion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("observaciones")
                        .HasColumnType("text");

                    b.Property<bool?>("peticionTransferencia")
                        .HasColumnType("boolean");

                    b.Property<string>("serie")
                        .HasColumnType("text");

                    b.Property<string>("unidad")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ArchivosAdministrativos");
                });
#pragma warning restore 612, 618
        }
    }
}
