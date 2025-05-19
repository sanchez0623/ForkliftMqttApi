using ForkliftMqtt.Domain.Services;
using ForkliftMqtt.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ForkliftMqtt.Infrastructure.Messaging
{
    public class MqttSensorDataService : ISensorDataService, IDisposable
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _options;
        private readonly ILogger<MqttSensorDataService> _logger;
        private readonly Dictionary<string, Action<SensorReading>> _subscriptions;
        //private readonly IJsonSerializer _serializer;
        private readonly string _topicPrefix;

        public MqttSensorDataService(
            IOptions<MqttSettings> settings,
            ILogger<MqttSensorDataService> logger)
            //IJsonSerializer serializer)
        {
            _logger = logger;
            //_serializer = serializer;
            _subscriptions = new Dictionary<string, Action<SensorReading>>();
            _topicPrefix = settings.Value.TopicPrefix;

            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithClientId(settings.Value.ClientId)
                .WithTcpServer(settings.Value.BrokerHost, settings.Value.BrokerPort)
                .WithCredentials(settings.Value.Username, settings.Value.Password)
                .WithTlsOptions(new MqttClientTlsOptions
                {
                    UseTls = true, // 启用TLS
                    //AllowUntrustedCertificates = false, // 是否允许不受信任的证书
                    //IgnoreCertificateChainErrors = false, // 是否忽略证书链错误
                    //IgnoreCertificateRevocationErrors = false, // 是否忽略证书吊销错误
                    CertificateValidationHandler = context =>
                    {
                        // 这里可以自定义证书验证逻辑，通常直接返回true即可通过
                        return true;
                    }
                })
                .WithCleanSession()
                .Build();

            _mqttClient.ConnectedAsync += HandleConnectedAsync;
            _mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
            _mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceivedAsync;

            ConnectAsync().GetAwaiter().GetResult();
        }

        private async Task ConnectAsync()
        {
            try
            {
                await _mqttClient.ConnectAsync(_options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to MQTT broker");
                throw;
            }
        }

        public async Task PublishSensorReadingAsync(SensorReading reading)
        {
            if (!_mqttClient.IsConnected)
            {
                await ConnectAsync();
            }

            var topic = $"{_topicPrefix}/sensors/{reading.SensorId}/data";
            var payload = JsonConvert.SerializeObject(reading); //_serializer.Serialize(reading);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag(false)
                .Build();

            await _mqttClient.PublishAsync(message);
            _logger.LogInformation($"Published reading to {topic}");
        }

        public async Task SubscribeToSensorAsync(string sensorId, Action<SensorReading> callback)
        {
            if (!_mqttClient.IsConnected)
            {
                await ConnectAsync();
            }

            var topic = $"{_topicPrefix}/sensors/{sensorId}/data";
            _subscriptions[sensorId] = callback;

            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build());

            _logger.LogInformation($"Subscribed to {topic}");
        }

        public async Task UnsubscribeFromSensorAsync(string sensorId)
        {
            if (!_mqttClient.IsConnected)
            {
                await ConnectAsync();
            }

            var topic = $"{_topicPrefix}/sensors/{sensorId}/data";

            await _mqttClient.UnsubscribeAsync(topic);
            _subscriptions.Remove(sensorId);

            _logger.LogInformation($"Unsubscribed from {topic}");
        }

        private Task HandleConnectedAsync(MqttClientConnectedEventArgs args)
        {
            _logger.LogInformation("Connected to MQTT broker");
            return Task.CompletedTask;
        }

        private Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs args)
        {
            _logger.LogWarning("Disconnected from MQTT broker: {Reason}", args.Reason);

            // 实现重连逻辑
            Task.Delay(TimeSpan.FromSeconds(5))
                .ContinueWith(_ => ConnectAsync())
                .ConfigureAwait(false);

            return Task.CompletedTask;
        }

        private Task HandleMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            try
            {
                var topic = args.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

                // 从Topic中提取传感器ID
                var match = Regex.Match(topic, $"{_topicPrefix}/sensors/(.+?)/data");
                if (match.Success)
                {
                    var sensorId = match.Groups[1].Value;
                    if (_subscriptions.TryGetValue(sensorId, out var callback))
                    {
                        var reading = JsonConvert.DeserializeObject<SensorReading>(payload); //_serializer.Deserialize<SensorReading>(payload);
                        callback(reading);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MQTT message");
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
        }
    }
}
