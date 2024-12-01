using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDNAPI.Models;
using System.Net.Http;
using CDNAPI.Interfaces;

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

        [HttpPost("TransformToAgoraFormat")]
        public async Task<IActionResult> TransformLog([FromBody] TransformLogRequest request)
        {
            var result = await _IEntityLogService.TransformLogFromRequest(request.Input, request.InputType, request.OutputFormat);

            return Ok(result);
        }

        [HttpPost("TransformLogSaved")]
        public async Task<IActionResult> TransformLogSaved([FromBody] TransformSavedRequest request)
        {
            var result = await _IEntityLogService.TransformLogSavedById(request.Id, request.OutputFormat);

            return Ok(result);
        }

        [HttpGet("saved")]
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
        [HttpGet("GetSavedLog/{id}")]
        public async Task<IActionResult> GetSavedLogById(Guid id)
        {
            var log = await _IEntityLogService.GetSavedLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log.MinhaCDNLog);
        }

        [HttpGet("GetTransformedLog/{id}")]
        public async Task<IActionResult> GetTransformedLogById(Guid id)
        {
            var log = await _IEntityLogService.GetTransformedLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("GetOriginalAndTransformed/{id}")]
        public async Task<IActionResult> GetOriginalAndTransformedById(Guid id)
        {
            var log = await _IEntityLogService.GetTransformedLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveLogMINHACDNFormat([FromBody] string content)
        {
            var entityLog = await _IEntityLogService.SaveLogMinhaCDNFormat(content);
            return Ok(entityLog.MinhaCDNLog);
        }
    }
}

public class TransformLogRequest
{
    public string Input { get; set; }
    public string InputType { get; set; }
    public string OutputFormat { get; set; }
}

public class TransformSavedRequest
{
    public Guid Id { get; set; }
    public string OutputFormat { get; set; }
}