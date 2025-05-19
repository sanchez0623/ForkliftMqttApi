using AutoMapper;
using ForkliftMqtt.Domain.Repositories;
using ForkliftMqtt.Domain.Services;
using ForkliftMqtt.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.Commands
{
    public class PublishSensorReadingCommandHandler : IRequestHandler<PublishSensorReadingCommand, bool>
    {
        private readonly ISensorDataService _sensorDataService;
        private readonly ISensorReadingRepository _readingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PublishSensorReadingCommandHandler> _logger;

        public PublishSensorReadingCommandHandler(
            ISensorDataService sensorDataService,
            ISensorReadingRepository readingRepository,
            IMapper mapper,
            ILogger<PublishSensorReadingCommandHandler> logger)
        {
            _sensorDataService = sensorDataService;
            _readingRepository = readingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Handle(PublishSensorReadingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var reading = new SensorReading(
                    request.SensorId,
                    request.Value,
                    request.Unit,
                    request.Attributes);

                await _sensorDataService.PublishSensorReadingAsync(reading);
                await _readingRepository.AddAsync(reading);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing sensor reading");
                return false;
            }
        }
    }
}
