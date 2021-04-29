using FinStar.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
	/// <summary>
	/// контроллер для выполнения необходимых методов
	/// </summary>
	[ApiController]
	[Route("api/v1")]
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

		/// <summary>
		/// Удаление старых записей и добавление новых
		/// </summary>
		/// <returns> status code </returns>
		[HttpPost("records")]
		public async Task<IActionResult> CreateRecords()
		{
			using var reader = new StreamReader(HttpContext.Request.Body);
			var body = await reader.ReadToEndAsync();
			var records = JsonConvert.DeserializeObject<List<Dictionary<int, string>>>(body)
				.Select(d => new Record { Code = d.Keys.FirstOrDefault(), Value = d.Values.FirstOrDefault() }).OrderBy(i => i.Code)
				.ToList();
			
			for (var i = 1; i <= records.Count(); i++)
				records[i - 1].Id = i;

			var oldRecords = _context.Records;
			_context.Records.RemoveRange(oldRecords);
			_logger.LogInformation("Old records was deleted");

			_context.Records.AddRange(records);
			_context.SaveChanges();

			return Created(nameof(RecordController), new { response = "Records was uploaded" });
		}

		/// <summary>
		/// Возвращает данные клиенту из таблицы в виде json. 
		/// </summary>
		/// <param name="pageNumber"> Номер возвращаемой страницы </param>
		/// <param name="pageCount"> Количество элементов на странице </param>
		/// <returns> status code </returns>
		[HttpGet("records")]
		public async Task<IActionResult> Get(int? pageNumber, int? pageCount)
		{
			if (pageCount == null || pageCount < 1)
			{
				pageCount = 20;
			}

			if (pageNumber == null || pageNumber < 1)
			{
				pageNumber = 1;
			}

			var count = await _context.Records.CountAsync();

			if (count == 0)
			{
				return NoContent();
			}

			var lastPage = (count - 1 + pageCount) / pageCount;

			if (lastPage < pageNumber)
			{
				pageNumber = lastPage;
			}

			var pageText = $"Page {pageNumber} from {lastPage}";

			var resultRecords = await _context.Records.Skip((pageNumber.Value - 1) * pageCount.Value).Take(pageCount.Value).ToListAsync();

			return Ok(new { pageInfo = pageText, records = resultRecords });
		}
	}
}
