using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinStar.Model
{
	public class RecordContext : DbContext
	{
		public RecordContext(DbContextOptions<RecordContext> options)
	   : base(options)
		{
		}

		public DbSet<Record> Records { get; set; }
	}
}
