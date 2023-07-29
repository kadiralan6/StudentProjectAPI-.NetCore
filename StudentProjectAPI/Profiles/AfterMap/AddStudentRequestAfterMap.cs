using AutoMapper;
using StudentProjectAPI.DataModels;
using StudentProjectAPI.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Profiles.AfterMap
{
    public class AddStudentRequestAfterMap: IMappingAction<AddStudentRequest, DataModels.Student>
    {
        public void Process(AddStudentRequest source, Student destination, ResolutionContext context)
        {
            destination.Id = Guid.NewGuid();
            destination.Adress = new DataModels.Adress()
            {
                Id = Guid.NewGuid(),
                PysicalAdress = source.PysicalAdress,
                PostalAdress = source.PostalAdress,
            };
        }
    }
}
