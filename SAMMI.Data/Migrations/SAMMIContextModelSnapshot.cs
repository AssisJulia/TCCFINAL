﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SAMMI.Data.Context;

#nullable disable

namespace SAMMI.Data.Migrations
{
    [DbContext(typeof(SAMMIContext))]
    partial class SAMMIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SAMMI.Domain.Entities.Pontuacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nivel")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("QuantidadePontos")
                        .HasColumnType("int");

                    b.Property<string>("TipoJogo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Pontuacao", (string)null);
                });

            modelBuilder.Entity("SAMMI.Domain.Entities.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("EmailLogin")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)");

                    b.HasKey("Id");

                    b.ToTable("TB_Usuario", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}