using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DnsGhost.Models
{
    public class DnsEntry
    {
        [Key]
        public Guid Id { get; set; }
        public string Token { set; get; }
        public string IpValue { set; get; }
        [ForeignKey("User")]
        public Guid User_Id { set; get; }
        public virtual User User { set; get; }
    }

    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { set; get; } 
    }

    public class DnsContext : DbContext
    {   
        public DbSet<DnsEntry> Entries { set; get; }
        public DbSet<User> Users { set; get; }
    }
}