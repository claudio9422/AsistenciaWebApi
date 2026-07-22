using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApiAsistencia.Models;

namespace WebApiAsistencia.Data;

public partial class DbAsistenciaContext : DbContext
{
    public DbAsistenciaContext()
    {
    }

    public DbAsistenciaContext(DbContextOptions<DbAsistenciaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Marcacione> Marcaciones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<TiposMarcacion> TiposMarcacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horarios__1539229B71FB5D8D");

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.NombreHorario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToleraEntradaMinutos).HasDefaultValue(15);
        });

        modelBuilder.Entity<Marcacione>(entity =>
        {
            entity.HasKey(e => e.IdMarcacion).HasName("PK__Marcacio__E45C3802D75B0754");

            entity.Property(e => e.FechaHoraRegistro).HasColumnType("datetime");
            entity.Property(e => e.LatitudMarcacion).HasColumnType("decimal(18, 15)");
            entity.Property(e => e.LongitudMarcacion).HasColumnType("decimal(18, 15)");
            entity.Property(e => e.RutaFoto)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TipoMarcacion)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Marcaciones)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("FK_Marcaciones_Sucursales");

            entity.HasOne(d => d.IdTipoMarcacionNavigation).WithMany(p => p.Marcaciones)
                .HasForeignKey(d => d.IdTipoMarcacion)
                .HasConstraintName("FK_Marcaciones_TiposMarcacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Marcaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Marcaciones_Usuarios");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584CCA6C516F");

            entity.HasIndex(e => e.NombreRol, "UQ__Roles__4F0B537FE3BFAAF0").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sucursale>(entity =>
        {
            entity.HasKey(e => e.IdSucursal).HasName("PK__Sucursal__BFB6CD995F659AEC");

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Latitud).HasColumnType("decimal(18, 15)");
            entity.Property(e => e.Longitud).HasColumnType("decimal(18, 15)");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RadioPermitidoMetros).HasDefaultValue(100);
        });

        modelBuilder.Entity<TiposMarcacion>(entity =>
        {
            entity.HasKey(e => e.IdTipoMarcacion).HasName("PK__TiposMar__1AD565BE203FF19C");

            entity.ToTable("TiposMarcacion");

            entity.HasIndex(e => e.Nombre, "UQ__TiposMar__75E3EFCFB3963F2D").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF975ECB3E79");

            entity.HasIndex(e => e.DocumentoIdentidad, "UQ__Usuarios__049E81A926147575").IsUnique();

            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BloqueadoPorGps).HasColumnName("BloqueadoPorGPS");
            entity.Property(e => e.DocumentoIdentidad)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.EsPrimerIngreso).HasDefaultValue(true);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.IntentosFallidosGps).HasColumnName("IntentosFallidosGPS");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("CAMBIAR_EN_EL_BACK");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Horarios");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Sucursales");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
