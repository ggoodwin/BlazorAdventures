using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.Utilities
{
    [Authorize]
    [Route("api/audit")]
    [ApiController]
    public class AuditsController : ControllerBase
    {
        //private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;

        public AuditsController(IAuditService auditService)
        {
            //_currentUserService = currentUserService;
            _auditService = auditService;
        }

        /// <summary>
        /// Get Current User Audit Trails
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.AuditTrails.View)]
        [HttpGet]
        public async Task<IActionResult> GetUserTrailsAsync()
        {
            return Ok(await _auditService.GetCurrentUserTrailsAsync("48541c9d-0c17-4e32-8cc1-dd12de643003"));
        }

        /// <summary>
        /// Search Audit Trails and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="searchInOldValues"></param>
        /// <param name="searchInNewValues"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.AuditTrails.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> ExportExcel(string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
        {
            var data = await _auditService.ExportToExcelAsync("48541c9d-0c17-4e32-8cc1-dd12de643003", searchString, searchInOldValues, searchInNewValues);
            return Ok(data);
        }
    }
}
