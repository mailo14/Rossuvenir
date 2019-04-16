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
            Mapper.CreateMap<SvetootrazatelNaklei, SvetootrazatelOneValueVM>();
            Mapper.CreateMap<ListItem, SvetootrazatelNaklei>()
            .ForMember(dest => dest.ZakazId,
               opts => opts.MapFrom(src => src.listId))
            .ForMember(dest => dest.Id,
               opts => opts.MapFrom(src => src.id))
            .ForMember(dest => dest.Tiraz,
               opts => opts.MapFrom(src => src.tiraz))
            .ForMember(dest => dest.Vid,
               opts => opts.MapFrom(src => src.param11));
        }
    }
}