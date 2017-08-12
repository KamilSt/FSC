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
            CreateMap<Customer, CustomersVM>();
            CreateMap<OrderItem, NewOrderItem>()
                .ForMember(x => x.Servis, opt => opt.MapFrom(no => no.ServiceItemName));
            CreateMap<NewOrderItem, OrderItem>()
                .ForMember(x => x.OrderId, opt => opt.Ignore())
                .ForMember(x => x.OrderItemId, opt => opt.MapFrom(no => no.OrderItemId))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(no => no.Quantity))
                .ForMember(x => x.Rate, opt => opt.MapFrom(no => no.Rate))
                .ForMember(x => x.ServiceItemName, opt => opt.MapFrom(no => no.Servis))
                .ForMember(x => x.ServiceItemCode, opt => opt.MapFrom(no => no.Servis))
                .ForMember(x => x.VAT, opt => opt.MapFrom(no => no.VAT));
            CreateMap<Order, NewOrderVM>()
                 .ForMember(x => x.Id, opt => opt.MapFrom(no => no.OrderId));
            CreateMap<NewOrderVM, Order>()
               .ForMember(v => v.OrderId, opt => opt.MapFrom(vr => vr.Id))
               .ForMember(o => o.Invoiced, opt => opt.Ignore())
               .ForMember(o => o.CustomerId, opt => opt.MapFrom(no => no.CustomerId))
               .ForMember(o => o.Description, opt => opt.MapFrom(no => no.Description))
               .ForMember(o => o.InvoiceDocuments, opt => opt.Ignore())
               .ForMember(o => o.OrderDateTime, opt => opt.Ignore())
               .ForMember(o => o.UserId, opt => opt.Ignore())
               .ForMember(v => v.OrderItems, opt => opt.Ignore());
        }
    }
}