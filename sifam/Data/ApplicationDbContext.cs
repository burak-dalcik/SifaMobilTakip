using Microsoft.EntityFrameworkCore;
using sifam.Models;
using static System.Net.Mime.MediaTypeNames;

namespace sifam.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } 
        public DbSet<Doctor> Doctors { get; set; } 
        public DbSet<Caregiver> Caregivers { get; set; } 
        public DbSet<Patient> Patients { get; set; } 
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Medication> Medications { get; set; } 
        public DbSet<Prescription> Prescriptions { get; set; } 
        public DbSet<Test> Tests { get; set; } 
        public DbSet<PatientActivity> PatientActivities { get; set; } 
        public DbSet<Pharmacy> Pharmacies { get; set; } 
        public DbSet<CaregiverRequest> CaregiverRequest { get; set; } 
        public DbSet<CaregiverRequest> CaregiverRequests { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.UserId).HasColumnName("user_id");
                entity.Property(u => u.PhoneNumber).HasColumnName("phone_number");
            });

            modelBuilder.Entity<CaregiverRequest>()
           .HasKey(c => c.RequestId);


            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patients");
                entity.Property(p => p.PatientId).HasColumnName("patient_id");
                entity.Property(p => p.UserId).HasColumnName("user_id");
                entity.Property(p => p.TcNo).HasColumnName("tc_no");
                entity.Property(p => p.BirthDate).HasColumnName("birth_date"); 
                entity.Property(p => p.Disease).HasColumnName("disease");
                entity.Property(p => p.Address).HasColumnName("address");

                entity.HasOne(p => p.User)
                    .WithMany()
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.Property(d => d.DoctorId).HasColumnName("doctor_id");
                entity.Property(d => d.UserId).HasColumnName("user_id");
                entity.HasOne(d => d.User)
                      .WithMany()
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Caregiver>(entity =>
            {
                entity.ToTable("Caregivers");
                entity.Property(c => c.CaregiverId).HasColumnName("caregiver_id");
                entity.Property(c => c.UserId).HasColumnName("user_id");
                entity.Property(c => c.AssignedPatientId).HasColumnName("assigned_patient_id");
                entity.HasOne(c => c.User)
                      .WithMany()
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(c => c.AssignedPatient)
                      .WithMany()
                      .HasForeignKey(c => c.AssignedPatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FamilyMember>(entity =>
            {
                entity.ToTable("FamilyMembers");
                entity.Property(fm => fm.FamilyMemberId).HasColumnName("family_member_id");
                entity.Property(fm => fm.UserId).HasColumnName("user_id");
                entity.Property(fm => fm.PatientId).HasColumnName("patient_id");
                entity.HasOne(fm => fm.User)
                      .WithMany()
                      .HasForeignKey(fm => fm.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(fm => fm.Patient)
                      .WithMany()
                      .HasForeignKey(fm => fm.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Medication>().ToTable("Medications");
            modelBuilder.Entity<Prescription>().ToTable("Prescriptions");
            modelBuilder.Entity<Test>().ToTable("Tests");
            modelBuilder.Entity<PatientActivity>().ToTable("PatientActivities");
            modelBuilder.Entity<Pharmacy>().ToTable("Pharmacies");
        }
    }
}
