using Domain.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Infrastructure.Context
{
    public partial class FormupContext
        : DbContext
    {
        public FormupContext()
        {
        }

        public FormupContext(DbContextOptions<FormupContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WorkCase> WorkCases { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<Forwarder> Forwarders { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceContractor> ServiceProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FormupContext).Assembly);
        }
    }
}
