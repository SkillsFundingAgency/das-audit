namespace SFA.DAS.Audit.Application.Mapping
{
    public class AutoMapperMapper : IMapper
    {
        private readonly AutoMapper.IMapper _mapper;

        public AutoMapperMapper(AutoMapper.IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDest Map<TDest>(object source)
        {
            return _mapper.Map<TDest>(source);
        }
    }
}
