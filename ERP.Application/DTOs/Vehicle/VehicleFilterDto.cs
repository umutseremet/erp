using ERP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleFilterDto
    {
        public string? SearchTerm { get; set; }
        public VehicleStatus? Status { get; set; }
        public VehicleType? Type { get; set; }
        public int? DepartmentId { get; set; }
        public int? UserId { get; set; }
        public int? Year { get; set; }
        public string? Brand { get; set; }
        public bool? HasActiveInsurance { get; set; }
        public bool? NeedsMaintenance { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
