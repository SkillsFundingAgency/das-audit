using AutoMapper;

namespace SFA.DAS.Audit.Web.Plumbing.Mapping
{
    public static class AutoMapperFactory
    {
        private static MapperConfiguration _mappingConfiguration;

        static AutoMapperFactory()
        {
            _mappingConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Types.Actor, Domain.Actor>().ReverseMap();
                cfg.CreateMap<Types.Entity, Domain.Entity>().ReverseMap();
                cfg.CreateMap<Types.PropertyUpdate, Domain.PropertyUpdate>().ReverseMap();
                cfg.CreateMap<Types.Source, Domain.Source>().ReverseMap();
                cfg.CreateMap<Types.AuditMessage, Domain.AuditMessage>().ReverseMap();
            });
        }

        public static IMapper CreateMapper()
        {
            return _mappingConfiguration.CreateMapper();
        }
    }
}