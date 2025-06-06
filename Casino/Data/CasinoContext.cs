using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Casino.Models;

namespace Casino.Data
{
    public class CasinoContext : DbContext
    {
        public CasinoContext (DbContextOptions<CasinoContext> options)
            : base(options)
        {
        }

        public DbSet<Casino.Models.Player> Player { get; set; } = default!;

        public DbSet<Casino.Models.Bet> Bet { get; set; } = default!;
        public DbSet<Casino.Models.PlayerArchive> Archive { get; set; } = default!;
		public DbSet<Casino.Models.History> History { get; set; } = default!;

	}
}
