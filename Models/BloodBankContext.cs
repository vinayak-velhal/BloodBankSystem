using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using BloodBankManagementSystem.Models;

namespace BloodBankManagementSystem.Models
{
    public class BloodBankContext:DbContext
    {
       
        public BloodBankContext(DbContextOptions<BloodBankContext>options):base(options)
        {
                
        }
        public DbSet<HospitalInformation> HospitalInformations { get; set; }
        public DbSet<DoctorInformation> DoctorInformations { get; set; }
        public DbSet<EmployeeInformation> EmployeeInformations { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<DonorRecord> DonorRecords { get; set; }
        public DbSet<Consent> Consents { get; set; }
        public DbSet<BloodInventory> BloodInventories { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonorRecord>().HasOne(d => d.Consent).WithOne(c=>c.Donor).HasForeignKey<Consent>(c => c.DonorId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BloodInventory>().HasOne(b => b.Donor).WithMany().HasForeignKey(b => b.DonorId).OnDelete(DeleteBehavior.SetNull);

        }




        public static implicit operator DbContextServices(BloodBankContext v)
        {
            throw new NotImplementedException();
        }
    }
}
