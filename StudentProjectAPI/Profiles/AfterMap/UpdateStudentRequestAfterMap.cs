using AutoMapper;
using StudentProjectAPI.DataModels;
using StudentProjectAPI.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.Profiles.AfterMap
{
    public class UpdateStudentRequestAfterMap : IMappingAction<UpdateStudentRequest, DataModels.Student>
    {
        public void Process(UpdateStudentRequest source, Student destination, ResolutionContext context)
        {
            destination.Adress = new DataModels.Adress()
            {
                PysicalAdress = source.PysicalAdress,
                PostalAdress = source.PostalAdress,
            };
        }
    }
}
