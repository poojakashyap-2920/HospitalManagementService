using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IDepartment
    {
        //adddept
        public Task<string> AddDepartment(DepartmentEntity departmentEntity);

        //getall
        public Task<List<DepartmentEntity>> GetAllDepartment();

        //delete 
        //  public Task DeleteDepartment(DepartmentEntity departmentEntity);


        //update
        // public Task UpdateDepartment(DepartmentEntity departmentEntity);


        //getById
        public Task<IEnumerable<DepartmentEntity>> GetDepartmentbyDeptId(string deptid);
    }

}
