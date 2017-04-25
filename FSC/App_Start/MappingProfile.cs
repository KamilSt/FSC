using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using FSC.DataLayer;
using FSC.ViewModels.Api;

namespace FSC.App_Start
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.AddProfile<WebApiProfile>();
            });
        }
    }

    public class WebApiProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Checklist, CheckListDisplayVM>().ForMember(d => d.Description,
                       opt => opt.MapFrom(src => src.Description)).ReverseMap();

            CreateMap<Checklist, CheckListItem>().ReverseMap();
        }
    }
}