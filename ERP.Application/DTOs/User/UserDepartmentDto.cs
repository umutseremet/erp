using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.DTOs.User
{
    public class UserDepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentCode { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StatusText => IsActive ? "Aktif" : "Pasif";
        public string AssignmentPeriod => EndDate.HasValue
            ? $"{AssignedDate:dd.MM.yyyy} - {EndDate.Value:dd.MM.yyyy}"
            : $"{AssignedDate:dd.MM.yyyy} - Devam ediyor";
    }

}
