using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlSanitize.Server.Persistance;
using SqlSanitize.Server.Services;
using SqlSanitize.Shared;

namespace SqlSanitize.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensitiveMessagesController : ControllerBase
    {
        private readonly SqlSanitizeDbContext _context;
        private readonly ISanitizer _sanitizer;
        private readonly ILogger _logger;

        public SensitiveMessagesController(SqlSanitizeDbContext context, ISanitizer sanitizer, ILogger<SensitiveMessagesController> logger)
        {
            _context = context;
            _sanitizer = sanitizer;
            _logger = logger;
        }

        // GET: api/SensitiveMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensitiveMessage>>> GetSensitiveMessages()
        {
            if (_context.SensitiveMessages == null)
            {
                return NotFound();
            }


            return await _context.SensitiveMessages.ToListAsync();
        }

        // GET: api/SensitiveMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SensitiveMessage>> GetSensitiveMessage(Guid id)
        {
            if (_context.SensitiveMessages == null)
            {
                return NotFound();
            }
            var sensitiveMessage = await _context.SensitiveMessages.FindAsync(id);

            if (sensitiveMessage == null)
            {
                return NotFound();
            }

            return sensitiveMessage;
        }

        // PUT: api/SensitiveMessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensitiveMessage(Guid id, SensitiveMessage sensitiveMessage)
        {
            if (id != sensitiveMessage.Id)
            {
                return BadRequest();
            }

            //sanitize message before saving in the DB
            sensitiveMessage.Message = await _sanitizer.SanitizeMessage(_logger, sensitiveMessage.Message, _context);

            if (string.IsNullOrEmpty(sensitiveMessage.Message))
                return Problem("Error encountered sanitizing message input!");

            _context.Entry(sensitiveMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensitiveMessageExists(id))
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

        // POST: api/SensitiveMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SensitiveMessage>> PostSensitiveMessage(SensitiveMessage sensitiveMessage)
        {
            if (_context.SensitiveMessages == null)
            {
                return Problem("Entity set 'SqlSanitizeDbContext.SensitiveMessages'  is null.");
            }

            //sanitize message before saving in the DB
            sensitiveMessage.Message = await _sanitizer.SanitizeMessage(_logger, sensitiveMessage.Message, _context);

            if (string.IsNullOrEmpty(sensitiveMessage.Message))
                return Problem("Error encountered sanitizing message input!");

            _context.SensitiveMessages.Add(sensitiveMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSensitiveMessage", new { id = sensitiveMessage.Id }, sensitiveMessage);
        }

        // DELETE: api/SensitiveMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensitiveMessage(Guid id)
        {
            if (_context.SensitiveMessages == null)
            {
                return NotFound();
            }
            var sensitiveMessage = await _context.SensitiveMessages.FindAsync(id);
            if (sensitiveMessage == null)
            {
                return NotFound();
            }

            _context.SensitiveMessages.Remove(sensitiveMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensitiveMessageExists(Guid id)
        {
            return (_context.SensitiveMessages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
