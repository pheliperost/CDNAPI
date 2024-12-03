using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CDNAPI.Interfaces;
using CDNAPI.ViewModels;

namespace CDNAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EntityLogsController : ControllerBase
    {
        private readonly IEntityLogRepository _entityLogRepository;
        private readonly IEntityLogService _IEntityLogService;
        
        public EntityLogsController(IEntityLogRepository entityLogRepository,
                                      IEntityLogService EntityLogService)
        {
            _entityLogRepository = entityLogRepository;
            _IEntityLogService = EntityLogService;
        }

        [HttpPost("TransformToAgoraFormatByRequest")]
        public async Task<IActionResult> TransformLog([FromBody] RequestLogViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _IEntityLogService.TransformLogFromRequest(request.URL,  request.OutputFormat);

            return Ok(result);
        }

        [HttpPost("TransformLogSavedById")]
        public async Task<IActionResult> TransformLogSaved([FromBody] TransformSavedViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _IEntityLogService.TransformLogSavedById(request.Id, request.OutputFormat);

            return Ok(result);
        }
        
        [HttpGet("SavedLogById/{id}")]
        public async Task<IActionResult> GetSavedLogById(Guid id)
        {
            var log = await _IEntityLogService.GetSavedLogById(id);
            if (log == null) return NotFound();
            return Ok(log.MinhaCDNLog);
        }

        [HttpGet("TransformedLogById/{id}")]
        public async Task<IActionResult> GetTransformedLogById(Guid id)
        {
            var log = await _IEntityLogService.GetTransformedLogById(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("OriginalAndTransformedLogById/{id}")]
        public async Task<IActionResult> GetOriginalAndTransformedLogById(Guid id)
        {
            var log = await _IEntityLogService.GetOriginalAndTransformedLogById(id);
            if (log == null) return NotFound();
            return Ok(log);
        }
    }
}
