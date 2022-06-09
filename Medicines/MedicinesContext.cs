using Medicines.Models;
using Microsoft.EntityFrameworkCore;

namespace Medicines
{
    public class MedicinesContext : DbContext
    {
        public MedicinesContext()
        {
        }

        public MedicinesContext(DbContextOptions<MedicinesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<Hospital> Hospitals { get; set; } = null!;
        public virtual DbSet<Medicine> Medicines { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<RecipesMedicine> RecipesMedicines { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Medicines;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasIndex(e => e.Id, "Doctors_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.License)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.HospitalId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Doctors_Hospitals_Id_fk");
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.HasIndex(e => e.Id, "Hospitals_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(55)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasIndex(e => e.Id, "Medicines_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(55)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(55)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasIndex(e => e.Id, "Patient_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(55)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasIndex(e => e.Id, "Recipes_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipes_Doctors_Id_fk");

                entity.HasMany(d => d.Patients)
                    .WithMany(p => p.Recipes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RecipesPatient",
                        l => l.HasOne<Patient>().WithMany().HasForeignKey("PatientId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("RecipesPatients_Patients_Id_fk"),
                        r => r.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("RecipesPatients_Recipes_Id_fk"),
                        j =>
                        {
                            j.HasKey("RecipeId", "PatientId").HasName("RecipesPatients_pk");

                            j.ToTable("RecipesPatients");
                        });
            });

            modelBuilder.Entity<RecipesMedicine>(entity =>
            {
                entity.HasKey(e => new { e.MedicineId, e.RecipeId })
                    .HasName("RecipesMedicines_pk");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.RecipesMedicines)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RecipesMedicines_Medicines_Id_fk");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipesMedicines)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RecipesMedicines_Recipes_Id_fk");
            });

        }
    }
}
