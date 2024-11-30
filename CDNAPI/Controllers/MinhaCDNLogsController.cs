﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDNAPI.Model;
using System.Net.Http;

namespace CDNAPI.Controllers
{
    [Route("api/MinhaCDN")]
    [ApiController]
    public class MinhaCDNLogsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        
        public MinhaCDNLogsController(ApiDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/MinhaCDNLogs
        [HttpGet]
        public IEnumerable<MinhaCDNLog> GetMinhaCDNLogs()
        {
            return _context.MinhaCDNLogs;
        }

        // GET: api/MinhaCDNLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMinhaCDNLog([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var minhaCDNLog = await _context.MinhaCDNLogs.FindAsync(id);

            if (minhaCDNLog == null)
            {
                return NotFound();
            }

            return Ok(minhaCDNLog);
        }

        // PUT: api/MinhaCDNLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMinhaCDNLog([FromRoute] Guid id, [FromBody] MinhaCDNLog minhaCDNLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != minhaCDNLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(minhaCDNLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MinhaCDNLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MinhaCDNLogs
        [HttpPost]
        public async Task<IActionResult> PostMinhaCDNLog([FromBody] MinhaCDNLog minhaCDNLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MinhaCDNLogs.Add(minhaCDNLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMinhaCDNLog", new { id = minhaCDNLog.Id }, minhaCDNLog);
        }

        // DELETE: api/MinhaCDNLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMinhaCDNLog([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var minhaCDNLog = await _context.MinhaCDNLogs.FindAsync(id);
            if (minhaCDNLog == null)
            {
                return NotFound();
            }

            _context.MinhaCDNLogs.Remove(minhaCDNLog);
            await _context.SaveChangesAsync();

            return Ok(minhaCDNLog);
        }

        private bool MinhaCDNLogExists(Guid id)
        {
            return _context.MinhaCDNLogs.Any(e => e.Id == id);
        }


        

        [HttpPost("ImportFromUrl")]
        public async Task<IActionResult> ProcessFile([FromBody] FileUrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Url))
            {
                return BadRequest("The file URL is required.");
            }

            try
            {
                // 1. Download the file from the URL
                var client = _httpClientFactory.CreateClient();
                var fileContent = await client.GetStringAsync(request.Url);

                // 2. Process the file content (e.g., reverse the text for demonstration)
                var processedContent = ProcessContent(fileContent);

                // 3. Return the processed content
                return Ok(new { ProcessedContent = processedContent });
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error fetching file: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        private string ProcessContent(string content)
        {
            // Example: Reverse the content
            var charArray = content.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}

public class FileUrlRequest
{
    public string Url { get; set; }
}