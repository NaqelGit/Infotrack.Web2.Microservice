using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Master.Microservice.Models
{
//add-migration
//update-database
public partial class MasterContext : DbContext
{
    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Region> Regions { get; set; }
    

    public virtual DbSet<Currency> Currencies { get; set; }
    


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID =postgres;Password=Sabitri@8019;Server=34.166.72.212;Database=Masters;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
}