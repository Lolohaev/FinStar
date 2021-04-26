using FinStar.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinStar.Controllers
{
	[ApiController]
	[Route("RecordController")]
	public class RecordController : ControllerBase
	{
		private readonly ILogger<RecordController> _logger;
		private Dictionary<int, string> values = new Dictionary<int, string>();
		private readonly RecordContext _context;

		public RecordController(RecordContext context, ILogger<RecordController> logger)
		{
			_context = context;
			_logger = logger;
		}

		[HttpPost("CreateRecords")]
		public async Task<IActionResult> CreateRecords()
		{
			using (var reader = new StreamReader(HttpContext.Request.Body))
			{
				var body = await reader.ReadToEndAsync();
				var records = JsonConvert.DeserializeObject<List<Dictionary<int, string>>>(body)
					.Select(d => new Record {Code = d.Keys.FirstOrDefault(), Value = d.Values.FirstOrDefault() }).OrderBy(i => i.Code)
					.ToList();

				for (var i = 1; i <= records.Count(); i++)
					records[i-1].Id = i;

				var oldRecords = _context.Records;
				_context.Records.RemoveRange(oldRecords);

				_context.Records.AddRange(records);
				_context.SaveChanges();
			}
			
			return Ok();
		}

		[HttpGet("Get")]
		public IActionResult Get()
		{
			return Ok();
		}
	}
}
