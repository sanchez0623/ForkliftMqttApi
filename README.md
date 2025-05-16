# 叉车MQTT系统架构说明

## 整体项目结构

项目结构

ForkliftMqtt.sln  
├── src/  
│ ├── ForkliftMqtt.Api/ - Web API入口项目  
│ ├── ForkliftMqtt.Application/- 应用服务层  
│ ├── ForkliftMqtt.Domain/ - 领域模型层  
│ └── ForkliftMqtt.Infrastructure/- 基础设施实现层  
└── tests/  
└── ForkliftMqtt.Tests/ - 单元测试项目  

## 1. API层 (ForkliftMqtt.Api)
```markdown
### 核心组件
- **Controllers/**
  - SensorsController.cs    - 传感器数据API
  - ForkliftController.cs   - 叉车状态控制API

- **Middleware/**
  - ExceptionHandlingMiddleware.cs - 全局异常处理
  - RequestLoggingMiddleware.cs    - 请求日志记录

- **Models/**
  - ErrorResponse.cs        - 标准错误响应模型
  - ApiResponse.cs          - 通用API响应包装器

- **Extensions/**
  - ServiceCollectionExtensions.cs - DI容器扩展方法

- **配置**
  - appsettings*.json       - 环境配置文件
  - launchSettings.json     - 启动参数配置

1. ForkliftMqtt.Api

ForkliftMqtt.Api/
├── Controllers/
│   ├── SensorsController.cs
│   └── ForkliftController.cs
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   └── RequestLoggingMiddleware.cs
├── Models/
│   ├── ErrorResponse.cs
│   └── ApiResponse.cs
├── Extensions/
│   └── ServiceCollectionExtensions.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── ForkliftMqtt.Api.csproj

2. ForkliftMqtt.Application

### 核心模块
- **Services/**
  - 接口层(Interfaces/)
    - ISensorApplicationService.cs
    - IForkliftApplicationService.cs
  - 实现层/
    - SensorApplicationService.cs    - 传感器业务逻辑
    - ForkliftApplicationService.cs  - 叉车业务逻辑

- **DTOs/**
  - ForkliftSensorDto.cs    - 传感器数据传输对象
  - SensorReadingDto.cs     - 传感器读数传输对象

- **CQRS模式**
  - Commands/              - 写操作命令
  - Queries/               - 读操作查询

- **支持组件**
  - Mapping/               - AutoMapper配置
  - Behaviors/             - MediatR管道行为
  - Validators/            - FluentValidation验证器

ForkliftMqtt.Application/
├── Services/
│   ├── Interfaces/
│   │   ├── ISensorApplicationService.cs
│   │   └── IForkliftApplicationService.cs
│   ├── SensorApplicationService.cs
│   └── ForkliftApplicationService.cs
├── DTOs/
│   ├── ForkliftSensorDto.cs
│   ├── SensorReadingDto.cs
│   └── CommandResponseDto.cs
├── Commands/
│   ├── PublishSensorReadingCommand.cs
│   └── MonitorSensorCommand.cs
├── Queries/
│   ├── GetSensorByIdQuery.cs
│   └── GetAllSensorsQuery.cs
├── Mapping/
│   └── MappingProfile.cs
├── Behaviors/
│   └── ValidationBehavior.cs
├── Validators/
│   ├── PublishSensorReadingCommandValidator.cs
│   └── MonitorSensorCommandValidator.cs
└── ForkliftMqtt.Application.csproj

3. ForkliftMqtt.Domain

### 领域模型
- **Entities/**
  - ForkliftSensor.cs      - 传感器实体（聚合根）
  - Forklift.cs            - 叉车实体

- **值对象**
  - SensorReading.cs       - 传感器读数（包含时间戳和值）
  - Location.cs            - 位置坐标对象

- **领域事件**
  - SensorDataReceivedEvent.cs     - 传感器数据接收事件
  - ForkliftStatusChangedEvent.cs  - 叉车状态变更事件

- **仓储接口**
  - ISensorRepository.cs   - 传感器仓储契约
  - IForkliftRepository.cs - 叉车仓储契约

- **领域服务**
  - ISensorDataService.cs  - 传感器数据处理服务接口

ForkliftMqtt.Domain/
├── Entities/
│   ├── ForkliftSensor.cs
│   └── Forklift.cs
├── ValueObjects/
│   ├── SensorReading.cs
│   └── Location.cs
├── Enums/
│   ├── SensorType.cs
│   └── ForkliftStatus.cs
├── Events/
│   ├── SensorDataReceivedEvent.cs
│   └── ForkliftStatusChangedEvent.cs
├── Exceptions/
│   ├── DomainException.cs
│   └── SensorNotFoundException.cs
├── Repositories/
│   ├── ISensorRepository.cs
│   ├── ISensorReadingRepository.cs
│   └── IForkliftRepository.cs
├── Services/
│   └── ISensorDataService.cs
└── ForkliftMqtt.Domain.csproj

4. ForkliftMqtt.Infrastructure

### 主要实现
- **消息通信**
  - MqttSensorDataService.cs - MQTT协议实现
  - MqttClientFactory.cs     - MQTT客户端工厂

- **数据持久化**
  - ForkliftDbContext.cs     - EF Core上下文
  - EntityConfigurations/   - 实体映射配置
  - Repositories/           - 仓储实现类

- **基础设施服务**
  - IJsonSerializer.cs       - JSON序列化抽象
  - LoggingService.cs        - 日志记录实现
  - InfrastructureServiceCollectionExtensions.cs - 基础设施DI注册

ForkliftMqtt.Infrastructure/
├── Messaging/
│   ├── MqttSensorDataService.cs
│   ├── MqttSettings.cs
│   └── MqttClientFactory.cs
├── Persistence/
│   ├── DbContext/
│   │   └── ForkliftDbContext.cs
│   ├── EntityConfigurations/
│   │   ├── ForkliftSensorConfiguration.cs
│   │   └── SensorReadingConfiguration.cs
│   └── Repositories/
│       ├── SensorRepository.cs
│       ├── SensorReadingRepository.cs
│       └── ForkliftRepository.cs
├── Serialization/
│   ├── IJsonSerializer.cs
│   └── SystemTextJsonSerializer.cs
├── Logging/
│   └── LoggingService.cs
├── Extensions/
│   └── InfrastructureServiceCollectionExtensions.cs
└── ForkliftMqtt.Infrastructure.csproj

