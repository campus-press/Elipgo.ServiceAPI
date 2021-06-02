using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Examen.Elipgo.DAL.Models;
using Examen.Elipgo.DAO.Models;

namespace Examen.Elipgo.DAO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArticleDAO, Article>().ReverseMap();
            CreateMap<StoreDAO, Store>().ReverseMap();
        }
    }
}
