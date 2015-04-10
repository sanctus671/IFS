namespace api.EModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Validation;

    public partial class EModelsContext : DbContext
    {
        public EModelsContext()
            : base("name=EModels")
        {
        }

        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<analysis_codes> analysis_codes { get; set; }
        public virtual DbSet<description> descriptions { get; set; }
        public virtual DbSet<request_admin_notes> request_admin_notes { get; set; }
        public virtual DbSet<request_items> request_items { get; set; }
        public virtual DbSet<request_payments> request_payments { get; set; }
        public virtual DbSet<request_permits> request_permits { get; set; }
        public virtual DbSet<request_status> request_status { get; set; }
        public virtual DbSet<request_suppliers> request_suppliers { get; set; }
        public virtual DbSet<request> requests { get; set; }
        public virtual DbSet<room> rooms { get; set; }
        public virtual DbSet<sharepoint_permissions> sharepoint_permissions { get; set; }
        public virtual DbSet<sharepoint_usergroups> sharepoint_usergroups { get; set; }
        public virtual DbSet<sharepoint_users> sharepoint_users { get; set; }
        public virtual DbSet<supplier> suppliers { get; set; }
        public virtual DbSet<tooltip> tooltips { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.GetType().ToString() + "." + x.PropertyName + ": " + x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<account>()
                .Property(e => e.number)
                .IsUnicode(false);

            modelBuilder.Entity<analysis_codes>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<analysis_codes>()
                .HasMany(e => e.requests)
                .WithOptional(e => e.analysis_codes)
                .HasForeignKey(e => e.codeid);

            modelBuilder.Entity<description>()
                .Property(e => e.description1)
                .IsUnicode(false);

            modelBuilder.Entity<description>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<description>()
                .HasMany(e => e.request_items)
                .WithOptional(e => e.description)
                .WillCascadeOnDelete();

            modelBuilder.Entity<request_admin_notes>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<request_items>()
                .Property(e => e.cas)
                .IsUnicode(false);

            modelBuilder.Entity<request_items>()
                .Property(e => e.quality)
                .IsUnicode(false);

            modelBuilder.Entity<request_items>()
                .Property(e => e.size)
                .IsUnicode(false);

            modelBuilder.Entity<request_payments>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<request_payments>()
                .Property(e => e.cost)
                .HasPrecision(8, 2);

            modelBuilder.Entity<request_payments>()
                .Property(e => e.pnnumber)
                .IsUnicode(false);

            modelBuilder.Entity<request_payments>()
                .Property(e => e.invoice)
                .IsUnicode(false);

            modelBuilder.Entity<request_permits>()
                .Property(e => e.number)
                .IsUnicode(false);

            modelBuilder.Entity<request_status>()
                .Property(e => e.date)
                .HasPrecision(0);

            modelBuilder.Entity<request_status>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<request>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<room>()
                .Property(e => e.room1)
                .IsUnicode(false);

            modelBuilder.Entity<sharepoint_permissions>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<sharepoint_permissions>()
                .HasMany(e => e.sharepoint_users)
                .WithOptional(e => e.sharepoint_permissions)
                .HasForeignKey(e => e.permissionid);

            modelBuilder.Entity<sharepoint_usergroups>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<sharepoint_usergroups>()
                .HasMany(e => e.sharepoint_users)
                .WithOptional(e => e.sharepoint_usergroups)
                .HasForeignKey(e => e.groupid);

            modelBuilder.Entity<sharepoint_users>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<sharepoint_users>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<sharepoint_users>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<sharepoint_users>()
                .HasMany(e => e.request_status)
                .WithRequired(e => e.sharepoint_users)
                .HasForeignKey(e => e.userid);

            modelBuilder.Entity<sharepoint_users>()
                .HasMany(e => e.requests)
                .WithRequired(e => e.sharepoint_users)
                .HasForeignKey(e => e.userid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<supplier>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<supplier>()
                .HasMany(e => e.request_suppliers)
                .WithRequired(e => e.supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tooltip>()
                .Property(e => e.field)
                .IsUnicode(false);

            modelBuilder.Entity<tooltip>()
                .Property(e => e.tooltip1)
                .IsUnicode(false);
        }
    }
}
