﻿using SoftJail.Data.Models;

namespace SoftJail.Data
{
	using Microsoft.EntityFrameworkCore;

	public class SoftJailDbContext : DbContext
	{
		public SoftJailDbContext()
		{
		}

		public SoftJailDbContext(DbContextOptions options)
			: base(options)
		{
		}

	    public DbSet<Cell> Cells { get; set; }
	    public DbSet<Department> Departments { get; set; }
	    public DbSet<Mail> Mails { get; set; }
	    public DbSet<Officer> Officers { get; set; }
	    public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }
	    public DbSet<Prisoner> Prisoners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

	    protected override void OnModelCreating(ModelBuilder builder)
	    {
	        builder.Entity<OfficerPrisoner>()
	            .HasKey(x => new {x.OfficerId, x.PrisonerId});

	        builder.Entity<OfficerPrisoner>()
	            .HasOne(e => e.Officer)
	            .WithMany(x => x.OfficerPrisoners)
	            .HasForeignKey(e => e.OfficerId)
	            .OnDelete(DeleteBehavior.Restrict);

	        builder.Entity<OfficerPrisoner>()
	            .HasOne(e => e.Prisoner)
	            .WithMany(x => x.PrisonerOfficers)
	            .HasForeignKey(e => e.PrisonerId)
	            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Prisoner>()
	            .Property(e => e.Bail)
	            .IsRequired(false);
        }

	}
}