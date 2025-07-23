//using ERP.Application.Common.Models;
//using ERP.Core.Enums;
//using MediatR;

//namespace ERP.Application.UseCases.Maintenance.Queries
//{
//    public class GetUpcomingMaintenanceQuery : IRequest<Result<IEnumerable<UpcomingMaintenanceDto>>>
//    {
//        public int DaysAhead { get; set; } = 30;
//        public int? VehicleId { get; set; }
//        public MaintenanceType? Type { get; set; }
//    }
//}