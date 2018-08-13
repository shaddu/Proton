using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductAPI.Mapper
{
    public class EntityMapper<TSource, TDestination> where TSource : class where TDestination : class
    {
        MapperConfiguration config;
        public EntityMapper()
        {
            config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Models.Product, DataAccessLayer.Product>();
                cfg.CreateMap<DataAccessLayer.Product, Models.Product>();
            });
        }

        public TDestination Translate(TSource obj)
        {
            IMapper mapper = config.CreateMapper();
            return mapper.Map<TDestination>(obj);
        }
    }
}