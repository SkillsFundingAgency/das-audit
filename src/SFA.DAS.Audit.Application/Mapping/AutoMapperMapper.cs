using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SFA.DAS.Audit.Application.Mapping
{
    public class AutoMapperMapper : IMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDest Map<TDest>(object source)
        {
            return _mapper.Map<TDest>(source);
        }
    }
}
