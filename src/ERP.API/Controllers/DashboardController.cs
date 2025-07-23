using ERP.Application.DTOs.Common;
using ERP.Application.Interfaces.Services;
using ERP.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IUserService _userService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IVehicleService vehicleService,
            IUserService userService,
            ILogger<DashboardController> logger)
        {
            _vehicleService = vehicleService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetDashboardStats()
        {
            try
            {
                var stats = new
                {
                    TotalVehicles = await _vehicleService.GetTotalVehicleCountAsync(),
                    AvailableVehicles = await _vehicleService.GetAvailableVehicleCountAsync(),
                    AssignedVehicles = await _vehicleService.GetAssignedVehicleCountAsync(),
                    MaintenanceVehicles = await _vehicleService.GetMaintenanceVehicleCountAsync(),
                    ActiveUsers = await _userService.GetActiveUserCountAsync()
                };

                return Ok(new ApiResponseDto<object>
                {
                    Data = stats,
                    Success = true,
                    Message = "Dashboard istatistikleri başarıyla getirildi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard istatistikleri getirilirken hata oluştu");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Dashboard istatistikleri getirilirken hata oluştu"
                });
            }
        }
    }
}