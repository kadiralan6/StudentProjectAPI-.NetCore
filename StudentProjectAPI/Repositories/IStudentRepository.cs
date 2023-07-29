using StudentProjectAPI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Repositories
{
   public interface IStudentRepository
    {
        
        Task< List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(Guid studentId);

        Task<List<Gender>> GetGenderAsync();
        Task<bool> Exists(Guid studentId);
        Task<Student> UpdateStudent(Guid studentId,Student student);
        Task<Student> AddStudent(Student student);
        Task<Student> DeleteStudent(Guid studentId);
        Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl);
    }
}
