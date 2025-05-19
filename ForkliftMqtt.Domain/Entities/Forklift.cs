using ForkliftMqtt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Domain.Entities
{
    public class Forklift
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Model { get; private set; }
        public ForkliftStatus Status { get; private set; }
        public DateTime LastMaintenanceDate { get; private set; }
        public List<ForkliftSensor> Sensors { get; private set; }

        private Forklift()
        {
            Sensors = new List<ForkliftSensor>();
        }

        public Forklift(string id, string name, string model, ForkliftStatus status = ForkliftStatus.Idle)
        {
            Id = id;
            Name = name;
            Model = model;
            Status = status;
            LastMaintenanceDate = DateTime.UtcNow;
            Sensors = new List<ForkliftSensor>();
        }

        public void UpdateStatus(ForkliftStatus newStatus)
        {
            Status = newStatus;
        }

        public void PerformMaintenance()
        {
            LastMaintenanceDate = DateTime.UtcNow;
        }

        public void AddSensor(ForkliftSensor sensor)
        {
            if (!Sensors.Any(s => s.Id == sensor.Id))
            {
                Sensors.Add(sensor);
            }
        }

        public void RemoveSensor(string sensorId)
        {
            var sensor = Sensors.FirstOrDefault(s => s.Id == sensorId);
            if (sensor != null)
            {
                Sensors.Remove(sensor);
            }
        }
    }
}
