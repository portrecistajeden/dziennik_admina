namespace dziennik_Admina.DataBaseAccess
{
    using dziennik_Admina.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class JournalDbContext : DbContext
    {
        public JournalDbContext() : base("name=JournalDbContext")
        {
        }
        public DbSet<JournalsList> Journals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Users_Roles> Users_Roles { get; set; }

        public DbSet<Journal1> Journal1 { get; set; }
        public DbSet<Journal2> Journal2 { get; set; }
        public DbSet<Journal3> Journal3 { get; set; }
    }
}