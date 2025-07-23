using Microsoft.AspNetCore.Mvc;
using ERP.Application.Services;
using ERP.Application.DTOs.Location;
using ERP.Application.DTOs.Common;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleLocationController : ControllerBase
    {
        private readonly IVehicleLocationService _locationService;
        private readonly ILogger<VehicleLocationController> _logger;

        public VehicleLocationController(IVehicleLocationService locationService, ILogger<VehicleLocationController> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        [HttpGet("{vehicleId}/current")]
        public async Task<ActionResult<VehicleLocationDto>> GetCurrentLocation(int vehicleId)
        {
            try
            {
                var location = await _locationService.GetCurrentLocationAsync(vehicleId);
                return Ok(new ApiResponseDto<VehicleLocationDto>
                {
                    Data = location,
                    Success = true,
                    Message = "Güncel konum başarıyla getirildi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç {VehicleId} güncel konum getirilirken hata oluştu", vehicleId);
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Güncel konum getirilirken hata oluştu"
                });
            }
        }

        [HttpGet("{vehicleId}/history")]
        public async Task<ActionResult<IEnumerable<LocationHistoryDto>>> GetLocationHistory(
            int vehicleId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var history = await _locationService.GetLocationHistoryAsync(vehicleId, startDate, endDate);
                return Ok(new ApiResponseDto<IEnumerable<LocationHistoryDto>>
                {
                    Data = history,
                    Success = true,
                    Message = "Konum geçmişi başarıyla getirildi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç {VehicleId} konum geçmişi getirilirken hata oluştu", vehicleId);
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Konum geçmişi getirilirken hata oluştu"
                });
            }
        }

        [HttpPost("{vehicleId}/location")]
        public async Task<ActionResult> UpdateLocation(int vehicleId, [FromBody] VehicleLocationDto locationDto)
        {
            try
            {
                await _locationService.UpdateLocationAsync(vehicleId, locationDto);
                return Ok(new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Konum başarıyla güncellendi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç {VehicleId} konum güncellenirken hata oluştu", vehicleId);
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Konum güncellenirken hata oluştu"
                });
            }
        }
    }
}