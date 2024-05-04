using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class DoctorObject
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }= string.Empty;
        public string Email { get; set; } = string.Empty;
        public int DeptId { get; set; } // Nullable string 
        public int DoctorAge { get; set; }
        public bool DoctorAvailability { get; set; }
        public string? DoctorQualifications { get; set; }  = string.Empty;
        public string? DoctorSpecialty { get; set; } = string.Empty;


    }

}
