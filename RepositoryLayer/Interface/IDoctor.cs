using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
   public  interface IDoctor
    {
        //create Doctor//authorized =doctor
        public  Task<string> AddDoctorDetail(string deptId, DoctorEntity doctorEntity);


        //Getall
         public Task<List<DoctorEntity>> GetAllDoctors();
        
        //getby id

        public Task<List<DoctorEntity>> GetDoctorById(int userid);

        //update //authorized =doctor
       //  public Task<DoctorEntity> UpdateDoctor(DoctorEntity doctorEntity);
        
        //delete
        public Task<DoctorEntity> DeleteDoctor(DoctorObject doctorObject ,int UserId);
     //  public  Task GetUserByUserId(DoctorInfo doctorInfo);
       // Task GetUserByUserId(int userId);
    }
}
