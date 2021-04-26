using FinStar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinStar.Interfaces
{
	interface IRecordRepository : IDisposable
	{
		void AddRecords(IEnumerable<Record> records);

		IEnumerable<Record> Getrecords();

		void Save();
	}
}
