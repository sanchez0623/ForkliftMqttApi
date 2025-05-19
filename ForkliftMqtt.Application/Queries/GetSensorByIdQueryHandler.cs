using AutoMapper;
using ForkliftMqtt.Application.DTOs;
using ForkliftMqtt.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.Queries
{
    public class GetSensorByIdQueryHandler : IRequestHandler<GetSensorByIdQuery, ForkliftSensorDto>
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly IMapper _mapper;

        public GetSensorByIdQueryHandler(
            ISensorRepository sensorRepository,
            IMapper mapper)
        {
            _sensorRepository = sensorRepository;
            _mapper = mapper;
        }

        public async Task<ForkliftSensorDto> Handle(GetSensorByIdQuery request, CancellationToken cancellationToken)
        {
            var sensor = await _sensorRepository.GetByIdAsync(request.Id);
            return _mapper.Map<ForkliftSensorDto>(sensor);
        }
    }
}
