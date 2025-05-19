using AutoMapper;
using ForkliftMqtt.Application.DTOs;
using ForkliftMqtt.Domain.Repositories;
using ForkliftMqtt.Domain.Services;
using ForkliftMqtt.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.Services
{
    public class SensorApplicationService : ISensorApplicationService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorDataService _sensorDataService;
        private readonly ISensorReadingRepository _readingRepository;
        private readonly IMapper _mapper;

        public SensorApplicationService(
            ISensorRepository sensorRepository,
            ISensorDataService sensorDataService,
            ISensorReadingRepository readingRepository,
            IMapper mapper)
        {
            _sensorRepository = sensorRepository;
            _sensorDataService = sensorDataService;
            _readingRepository = readingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ForkliftSensorDto>> GetAllSensorsAsync()
        {
            var sensors = await _sensorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ForkliftSensorDto>>(sensors);
        }

        public async Task<ForkliftSensorDto> GetSensorByIdAsync(string id)
        {
            var sensor = await _sensorRepository.GetByIdAsync(id);
            return _mapper.Map<ForkliftSensorDto>(sensor);
        }

        public async Task<bool> PublishSensorReadingAsync(SensorReadingDto readingDto)
        {
            var reading = _mapper.Map<SensorReading>(readingDto);

            // 发布到MQTT
            await _sensorDataService.PublishSensorReadingAsync(reading);

            // 保存到数据库
            await _readingRepository.AddAsync(reading);

            return true;
        }

        public async Task<bool> StartMonitoringSensorAsync(string sensorId)
        {
            await _sensorDataService.SubscribeToSensorAsync(sensorId, async (reading) => {
                // 当接收到传感器数据时
                await _readingRepository.AddAsync(reading);
                // 这里可以添加其他业务逻辑，如触发警报等
            });

            return true;
        }

        public async Task<bool> StopMonitoringSensorAsync(string sensorId)
        {
            await _sensorDataService.UnsubscribeFromSensorAsync(sensorId);
            return true;
        }
    }
}
