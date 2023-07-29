using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using StudentProjectAPI.DomainModels;
using StudentProjectAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Controllers
{
    [ApiController]
    public class GendersController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        public GendersController(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;

        }
       
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllGendersAsync()
        {
            var genderList = await studentRepository.GetGenderAsync();
            if(genderList==null || !genderList.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<GenderDto>>(genderList)); ;
        }
    }
}
