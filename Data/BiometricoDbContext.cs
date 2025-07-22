using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BiomentricoHolding.Data;

public partial class BiometricoDbContext : DbContext
{
    public BiometricoDbContext()
    {
    }

    public BiometricoDbContext(DbContextOptions<BiometricoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AsignacionHorario> AsignacionHorarios { get; set; }

    public virtual DbSet<Cache> Caches { get; set; }

    public virtual DbSet<CacheLock> CacheLocks { get; set; }

    public virtual DbSet<Ciudad> Ciudads { get; set; }

    public virtual DbSet<DetalleHorario> DetalleHorarios { get; set; }

    public virtual DbSet<DiasDeLaSemana> DiasDeLaSemanas { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<FailedJob> FailedJobs { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobBatch> JobBatches { get; set; }

    public virtual DbSet<Marcacione> Marcaciones { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }

    public virtual DbSet<RolPermiso> RolPermisos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<SedeArea> SedeAreas { get; set; }

    public virtual DbSet<SedeCiudadEmpresaArea> SedeCiudadEmpresaAreas { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<TiposEmpleado> TiposEmpleados { get; set; }

    public virtual DbSet<TiposMarcacion> TiposMarcacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=190.71.223.36;Database=BiometricoDesarrollo;User Id=Desarrollador;Password=$Metrico$Bio$;TrustServerCertificate=True;Encrypt=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.IdArea);

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AsignacionHorario>(entity =>
        {
            entity.Property(e => e.Documento).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
        });

        modelBuilder.Entity<Cache>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("cache");

            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<CacheLock>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("cache_locks");

            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.Owner)
                .HasMaxLength(255)
                .HasColumnName("owner");
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.HasKey(e => e.IdCiudad);

            entity.ToTable("Ciudad");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DetalleHorario>(entity =>
        {
            entity.HasOne(d => d.DiaSemanaNavigation).WithMany(p => p.DetalleHorarios)
                .HasForeignKey(d => d.DiaSemana)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleHorarios_DiasDeLaSemana");

            entity.HasOne(d => d.IdAsignacionNavigation).WithMany(p => p.DetalleHorarios)
                .HasForeignKey(d => d.IdAsignacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleHorarios_AsignacionHorarios1");
        });

        modelBuilder.Entity<DiasDeLaSemana>(entity =>
        {
            entity.ToTable("DiasDeLaSemana");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(10)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK_Empleados_1");

            entity.HasIndex(e => e.Documento, "UQ_Empleados_Documento").IsUnique();

            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Documento).HasMaxLength(50);
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaIngreso).HasColumnType("datetime");
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa);

            entity.ToTable("Empresa");

            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FailedJob>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("failed_jobs");

            entity.Property(e => e.Connection).HasColumnName("connection");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.FailedAt)
                .HasColumnType("datetime")
                .HasColumnName("failed_at");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue).HasColumnName("queue");
            entity.Property(e => e.Uuid)
                .HasMaxLength(255)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("jobs");

            entity.Property(e => e.Attempts).HasColumnName("attempts");
            entity.Property(e => e.AvailableAt).HasColumnName("available_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue)
                .HasMaxLength(255)
                .HasColumnName("queue");
            entity.Property(e => e.ReservedAt).HasColumnName("reserved_at");
        });

        modelBuilder.Entity<JobBatch>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("job_batches");

            entity.Property(e => e.CancelledAt).HasColumnName("cancelled_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FailedJobIds).HasColumnName("failed_job_ids");
            entity.Property(e => e.FailedJobs).HasColumnName("failed_jobs");
            entity.Property(e => e.FinishedAt).HasColumnName("finished_at");
            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.PendingJobs).HasColumnName("pending_jobs");
            entity.Property(e => e.TotalJobs).HasColumnName("total_jobs");
        });

        modelBuilder.Entity<Marcacione>(entity =>
        {
            entity.Property(e => e.Documento).HasMaxLength(50);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");

            entity.HasOne(d => d.IdTipoMarcacionNavigation).WithMany(p => p.Marcaciones)
                .HasForeignKey(d => d.IdTipoMarcacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Marcaciones_TiposMarcacion");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("migrations");

            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Migration1)
                .HasMaxLength(255)
                .HasColumnName("migration");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.Property(e => e.Accion).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.Modulo).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<PersonalAccessToken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("personal_access_tokens");

            entity.Property(e => e.Abilities).HasColumnName("abilities");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.LastUsedAt)
                .HasColumnType("datetime")
                .HasColumnName("last_used_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .HasColumnName("token");
            entity.Property(e => e.TokenableId).HasColumnName("tokenable_id");
            entity.Property(e => e.TokenableType)
                .HasMaxLength(255)
                .HasColumnName("tokenable_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<RolPermiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolPermi__3214EC07044A607E");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.IdPermiso).HasColumnName("Id_permiso");
            entity.Property(e => e.IdRol).HasColumnName("Id_rol");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolPermisos_Permisos");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolPermisos_Roles");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.Property(e => e.CreatedBy).HasColumnName("Created_by");
            entity.Property(e => e.EsEditable).HasColumnName("Es_editable");
            entity.Property(e => e.Rol).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__Sede__A7780DFF134404BB");

            entity.Property(e => e.IdSede).ValueGeneratedNever();
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SedeArea>(entity =>
        {
            entity.HasKey(e => new { e.IdSede, e.IdArea });

            entity.ToTable("SedeArea");
        });

        modelBuilder.Entity<SedeCiudadEmpresaArea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SedeCiud__3214EC077D9AA34B");

            entity.ToTable("SedeCiudadEmpresaArea");

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.SedeCiudadEmpresaAreas)
                .HasForeignKey(d => d.IdArea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SedeCiudadEmpresaArea_Area");

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.SedeCiudadEmpresaAreas)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SedeCiudadEmpresaArea_Ciudad");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.SedeCiudadEmpresaAreas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SedeCiudadEmpresaArea_Empresa");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.SedeCiudadEmpresaAreas)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SedeCiudadEmpresaArea_Sedes");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("sessions");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.LastActivity).HasColumnName("last_activity");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<TiposEmpleado>(entity =>
        {
            entity.ToTable("TiposEmpleado");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposMarcacion>(entity =>
        {
            entity.ToTable("TiposMarcacion");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario");

            entity.Property(e => e.Contrasena).HasMaxLength(100);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRolUsuario)
                .HasConstraintName("FK_Usuario_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
