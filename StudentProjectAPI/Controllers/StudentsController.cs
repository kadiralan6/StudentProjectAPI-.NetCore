using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentProjectAPI.DataModels;
using StudentProjectAPI.DomainModels;
using StudentProjectAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository IImageRepository;
        public StudentsController(IStudentRepository studentRepository,IMapper mapper, IImageRepository IImageRepository)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.IImageRepository = IImageRepository;
        }

        //buradaki kod mapleme işlemi yapılmadan yapılan olay
        //[HttpGet]
        //[Route("[controller]")]
        //public IActionResult GetAllStudents ()
        //{
        //    var students= studentRepository.GetStudents();

        //    //domainModelsStudent
        //    var domainModelStudents = new List<StudentDto>();
        //    foreach (var item in students)
        //    {
        //        domainModelStudents.Add(new StudentDto()
        //        {
        //            Id = item.Id,
        //            FirstName = item.FirstName,
        //            LastName = item.LastName,
        //            DateOfBirth = item.DateOfBirth,
        //            Email = item.Email,
        //            Mobile = item.Mobile,
        //            GenderId = item.GenderId,
        //            ProfileImageUrl = item.ProfileImageUrl,
        //            Adress = new AdressDto()
        //            {
        //                Id = item.Adress.Id,
        //                PysicalAdress = item.Adress.PysicalAdress,
        //                PostalAdress = item.Adress.PostalAdress,
        //            },
        //            Gender = new GenderDto()
        //            {
        //                Id = item.Gender.Id,
        //                Description = item.Gender.Description,
        //            }

        //        }) ;
        //    }

        //    return Ok(domainModelStudents);
        //}

        [HttpGet]
        [Route("[controller]")]
        public async Task< IActionResult> GetAllStudentsAsync()
        {
            var students = await studentRepository.GetStudentsAsync();

            return Ok(mapper.Map<List<StudentDto>>(students));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}"),ActionName ("GetAllStudentAsync")]
        public async Task<IActionResult> GetAllStudentAsync([FromRoute] Guid studentId)
        {
            var student = await studentRepository.GetStudentAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<StudentDto>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId,[FromBody] UpdateStudentRequest request)
        {

            if (await studentRepository.Exists(studentId))
            {
                var updateStudent = studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));
                if (updateStudent != null)
                {
                    return Ok(mapper.Map<StudentDto>(updateStudent));
                }    
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {

            if (await studentRepository.Exists(studentId))
            {
                var student = studentRepository.DeleteStudent(studentId);
                if (student != null)
                {
                    return Ok(mapper.Map<StudentDto>(student));
                }
            }
            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddStudentAsync([FromBody] AddStudentRequest request)
        {
           var student = await studentRepository.AddStudent(mapper.Map<DataModels.Student>(request));

            return CreatedAtAction(nameof(GetAllStudentAsync),new { student=student.Id},mapper.Map<Student>(student));
    
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
               ".jpeg",
               ".png",
               ".gif",
               ".jpg"
            };

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    if (await studentRepository.Exists(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await IImageRepository.Upload(profileImage, fileName);

                        if (await studentRepository.UpdateProfileImage(studentId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }
                }

                return BadRequest("This is not a valid Image format");
            }

            return NotFound();
        }


    }
}
