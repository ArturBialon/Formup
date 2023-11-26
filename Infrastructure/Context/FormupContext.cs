using Infrastructure.Models;
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

        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Cost> Costs { get; set; }
        public virtual DbSet<Forwarder> Forwarders { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceProvider> ServiceProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Case>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ForwardersId).HasColumnName("Forwarders_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Relation)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.Forwarders)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.ForwardersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cases_Forwarders");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Coutry)
                    .IsRequired()
                    .HasMaxLength(54)
                    .IsUnicode(false);

                entity.Property(e => e.Credit).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Tax)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TAX");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ZIP");
            });

            modelBuilder.Entity<Cost>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CasesId).HasColumnName("Cases_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceProvidersId).HasColumnName("Service_Providers_ID");

                entity.Property(e => e.Tax).HasColumnName("TAX");

                entity.HasOne(d => d.Cases)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.CasesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Costs_Cases");

                entity.HasOne(d => d.ServiceProviders)
                    .WithMany(p => p.Costs)
                    .HasForeignKey(d => d.ServiceProvidersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Costs_Service_Providers");
            });

            modelBuilder.Entity<Forwarder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Prefix)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PassHash)
                    .IsRequired()
                    .HasColumnType("varbinary")
                    .IsUnicode(false);

                entity.Property(e => e.PassSalt)
                    .IsRequired()
                    .HasColumnType("varbinary")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.CasesId).HasColumnName("Cases_ID");

                entity.Property(e => e.ClientsId).HasColumnName("Clients_ID");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("date")
                    .HasColumnName("Issue_Date");

                entity.Property(e => e.ServiceDate)
                    .HasColumnType("date")
                    .HasColumnName("Service_Date");

                entity.Property(e => e.Tax).HasColumnName("TAX");

                entity.HasOne(d => d.Cases)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.CasesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Invoices_Cases");

                entity.HasOne(d => d.Clients)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.ClientsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Invoices_Clients");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amonut).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.InvoicesId).HasColumnName("Invoices_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Tax).HasColumnName("TAX");

                entity.HasOne(d => d.Invoices)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.InvoicesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Service_Invoices");
            });

            modelBuilder.Entity<ServiceProvider>(entity =>
            {
                entity.ToTable("Service_Providers");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Coutry)
                    .IsRequired()
                    .HasMaxLength(54)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Tax)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TAX");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ZIP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
