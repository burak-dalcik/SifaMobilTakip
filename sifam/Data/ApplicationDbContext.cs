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
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<PatientActivity> PatientActivities { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<CaregiverRequest> CaregiverRequests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageParticipant> MessageParticipants { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CaregiverRequest>()
                .HasKey(c => c.RequestId);

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patients");

                entity.HasOne(p => p.User)
                    .WithMany()
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Caregiver>(entity =>
            {
                entity.ToTable("Caregivers");

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

                entity.HasOne(fm => fm.User)
                    .WithMany()
                    .HasForeignKey(fm => fm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(fm => fm.Patient)
                    .WithMany()
                    .HasForeignKey(fm => fm.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Medication>(entity =>
            {
                entity.ToTable("Medications"); // Tablo adı

                entity.HasKey(m => m.MedicationId); // Primary Key
            });

            modelBuilder.Entity<Message>()
                .HasMany(m => m.Participants)
                .WithOne(mp => mp.Message)
                .HasForeignKey(mp => mp.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageParticipant>()
                .HasKey(mp => new { mp.MessageId, mp.UserId });

            modelBuilder.Entity<MessageParticipant>()
                .HasOne(mp => mp.User)  // User ilişkisi
                .WithMany()  // User modelinde MessageParticipant'a bir koleksiyon yok
                .HasForeignKey(mp => mp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Doctor)
               .WithMany(d => d.Appointments)
               .HasForeignKey(a => a.DoctorId);

            // Patient - Appointment İlişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Prescription>().ToTable("Prescriptions");
            modelBuilder.Entity<Test>().ToTable("Tests");
            modelBuilder.Entity<PatientActivity>().ToTable("PatientActivities");
            modelBuilder.Entity<Pharmacy>().ToTable("Pharmacies");
        }
    }
}
