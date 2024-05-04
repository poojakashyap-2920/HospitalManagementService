using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
   public  class DepartmentEntity
    {
        [Key]
        public int DeptId { get; set; }
        public string DeptName { get; set; }

        public string SpecialtyDepartment {  get; set; }


    } 
}
