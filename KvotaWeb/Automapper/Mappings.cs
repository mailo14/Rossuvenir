using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using KvotaWeb.Models;
using KvotaWeb.Models.Items;
using KvotaWeb.ViewModels;

namespace KvotaWeb.Automapper
{
    public static class Mappings
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<ListItem, SvetootrazatelNaklei>()
            .ForMember(dest => dest.TipProd,
               opts => opts.MapFrom(src => (TipProds)src.tipProd))
            .ForMember(dest => dest.ZakazId,
               opts => opts.MapFrom(src => src.listId))
            .ForMember(dest => dest.Id,
               opts => opts.MapFrom(src => src.id))
            .ForMember(dest => dest.Tiraz,
               opts => opts.MapFrom(src => src.tiraz))
            .ForMember(dest => dest.Vid,
               opts => opts.MapFrom(src => src.param11));
            Mapper.CreateMap<SvetootrazatelNaklei, SvetootrazatelOneValueVM>()
            .ForMember(dest => dest.Source,
               opts => opts.MapFrom(src => src));
            Mapper.CreateMap<SvetootrazatelOneValueVM, ListItem > ()
            .ForMember(dest => dest.tipProd,
               opts => opts.MapFrom(src => (int)src.TipProd))
            .ForMember(dest => dest.listId,
               opts => opts.MapFrom(src => src.ZakazId))
            .ForMember(dest => dest.id,
               opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.tiraz,
               opts => opts.MapFrom(src => src.Tiraz))
            .ForMember(dest => dest.param11,
               opts => opts.MapFrom(src => src.Vid));
        }
    }
}