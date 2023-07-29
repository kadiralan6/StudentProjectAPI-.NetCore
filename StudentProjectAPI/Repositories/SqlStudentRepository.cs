using Microsoft.EntityFrameworkCore;
using StudentProjectAPI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly ProjectContext context;
        public SqlStudentRepository(ProjectContext context)
        {
            this.context = context;

        }

    

        public async Task < List<Student> > GetStudentsAsync()
        {
           // return context.Student.ToList(); // bu satır sadece studentları getiriyor
           //alttak kodda ise navigation propertyleri de dahil ederek çekiyoruz.
            return  await context.Student.Include(nameof(Gender)).Include(nameof(Adress)).ToListAsync(); 
        }
        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Adress)).FirstOrDefaultAsync(a=>a.Id==studentId);

        }

        public  async Task<List<Gender>> GetGenderAsync()
        {
            return await context.Gender.ToListAsync();
        }

        public async Task<bool> Exists(Guid studentId)
        {
         return await context.Student.AnyAsync(a=>a.Id==studentId);
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if(existingStudent != null)
            {
                existingStudent.FirstName = request.FirstName;
                existingStudent.LastName = request.LastName;
                existingStudent.DateOfBirth = request.DateOfBirth;
                existingStudent.Email = request.Email;
                existingStudent.Mobile = request.Mobile;
                existingStudent.GenderId = request.GenderId;
                existingStudent.Adress.PysicalAdress = request.Adress.PysicalAdress;
                existingStudent.Adress.PostalAdress = request.Adress.PostalAdress;
                await context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }

        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                context.Student.Remove(existingStudent);
                await context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }

        public async Task<Student> AddStudent(Student request)
        {
           var student=  await context.Student.AddAsync(request);

            await context.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                student.ProfileImageUrl = profileImageUrl;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //Task<List<Student>> IStudentRepository.GetStudentsAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
