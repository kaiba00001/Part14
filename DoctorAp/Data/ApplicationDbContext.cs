using DoctorAp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorAp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BookingLeadEntity> BookingLead { get; set; }


        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        


        public DbSet<DoctorAp.Models.ScreenLead>? ScreenLead_1 { get; set; }


        public DbSet<PrescriptionLead> PrescriptionLead { get; set; }

        public DbSet<SupplyLead> SupplyLead { get; set;}

        public DbSet<ItemsLead> ItemsLead{ get; set; }
    }
}
