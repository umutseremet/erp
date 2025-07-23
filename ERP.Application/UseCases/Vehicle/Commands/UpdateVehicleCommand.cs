using ERP.Core.Enums;
using ERP.Application.DTOs.Vehicle;
using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Commands
{
    public class UpdateVehicleCommand : IRequest<Result<VehicleDto>>
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string VinNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Color { get; set; }
        public VehicleType Type { get; set; }
        public FuelType FuelType { get; set; }
        public decimal CurrentKm { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal EngineSize { get; set; }
        public string? Notes { get; set; }
    }
}