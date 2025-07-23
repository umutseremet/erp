using Microsoft.AspNetCore.Mvc;
using ERP.Application.Services;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.DTOs.Common;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<VehicleListDto>>> GetVehicles(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            try
            {
                var result = await _vehicleService.GetVehiclesAsync(page, pageSize, search);
                return Ok(new ApiResponseDto<PagedResultDto<VehicleListDto>>
                {
                    Data = result,
                    Success = true,
                    Message = "Araçlar başarıyla getirildi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araçlar getirilirken hata oluştu");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Araçlar getirilirken hata oluştu"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDetailDto>> GetVehicle(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    return NotFound(new ApiResponseDto<object>
                    {
                        Success = false,
                        Message = "Araç bulunamadı"
                    });
                }

                return Ok(new ApiResponseDto<VehicleDetailDto>
                {
                    Data = vehicle,
                    Success = true,
                    Message = "Araç başarıyla getirildi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç {VehicleId} getirilirken hata oluştu", id);
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Araç getirilirken hata oluştu"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> CreateVehicle([FromBody] CreateVehicleDto createVehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.CreateVehicleAsync(createVehicleDto);
                return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id },
                    new ApiResponseDto<VehicleDto>
                    {
                        Data = vehicle,
                        Success = true,
                        Message = "Araç başarıyla oluşturuldu"
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç oluşturulurken hata oluştu");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Araç oluşturulurken hata oluştu"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(int id, [FromBody] UpdateVehicleDto updateVehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.UpdateVehicleAsync(id, updateVehicleDto);
                return Ok(new ApiResponseDto<VehicleDto>
                {
                    Data = vehicle,
                    Success = true,
                    Message = "Araç başarıyla güncellendi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç {VehicleId} güncellenirken hata oluştu", id);
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Araç güncellenirken hata oluştu"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return Ok(new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Araç başarıyla silindi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç {VehicleId} silinirken hata oluştu", id);
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Araç silinirken hata oluştu"
                });
            }
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<VehicleListDto>>> GetAvailableVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAvailableVehiclesAsync();
                return Ok(new ApiResponseDto<IEnumerable<VehicleListDto>>
                {
                    Data = vehicles,
                    Success = true,
                    Message = "Müsait araçlar başarıyla getirildi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Müsait araçlar getirilirken hata oluştu");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Müsait araçlar getirilirken hata oluştu"
                });
            }
        }

        [HttpGet("{id}/tracking")]
        public async Task<ActionResult<object>> GetVehicleTracking(int id)
        {
            try
            {
                // Bu metod araç takip ekranı için gerekli tüm bilgileri getirecek
                var tracking = await _vehicleService.GetVehicleTrackingInfoAsync(id);
                return Ok(new ApiResponseDto<object>
                {
                    Data = tracking,
                    Success = true,
                    Message = "Araç takip bilgileri başarıyla getirildi"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Araç {VehicleId} takip bilgileri getirilirken hata oluştu", id);
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Araç takip bilgileri getirilirken hata oluştu"
                });
            }
        }
    }
}