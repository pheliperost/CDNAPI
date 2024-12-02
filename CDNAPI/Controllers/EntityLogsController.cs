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
        public async Task<IActionResult> TransformLog([FromBody] TransformLogViewModel request)
        {
            var result = await _IEntityLogService.TransformLogFromRequest(request.URL,  request.OutputFormat);

            return Ok(result);
        }

        [HttpPost("TransformLogSavedById")]
        public async Task<IActionResult> TransformLogSaved([FromBody] TransformSavedRequestViewModel request)
        {
            var result = await _IEntityLogService.TransformLogSavedById(request.Id, request.OutputFormat);

            return Ok(result);
        }

        //verificar eventual melhoria no retorno
        [HttpGet("GetAllSavedLogs")]
        public async Task<IActionResult> GetSavedLogs()
        {
            var logs = await _IEntityLogService.GetSavedLogsAsync();
            return Ok(logs);
        }

        //verificar eventual melhoria no retorno
        [HttpGet("GetAllTransformedLogs")]
        public async Task<IActionResult> GetTransformedLogs()
        {
            var logs = await _IEntityLogService.GetTransformedLogsAsync();
            return Ok(logs);
        }

        //log saved but not transformed
        [HttpGet("GetSavedLogById/{id}")]
        public async Task<IActionResult> GetSavedLogById([FromRoute] LogSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var log = await _IEntityLogService.GetSavedLogByIdAsync(viewModel.Id);
            if (log == null) return NotFound();
            return Ok(log.MinhaCDNLog);
        }

        [HttpGet("GetTransformedLogById/{id}")]
        public async Task<IActionResult> GetTransformedLogById([FromRoute] LogSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var log = await _IEntityLogService.GetTransformedLogByIdAsync(viewModel.Id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("GetOriginalAndTransformedLogById/{id}")]
        public async Task<IActionResult> GetOriginalAndTransformedLogById([FromRoute] LogSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var log = await _IEntityLogService.GetTransformedLogByIdAsync(viewModel.Id);
            if (log == null) return NotFound();
            return Ok(log);
        }
    }
}
