using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtisElevatorDevice.Models
{
    public class ElevatorListItem
    {

        public string? Id { get; set; } = null;
        public string? Location { get; set; }  = null;
        public string? ElevatorStatus { get; set; }
        public string? ElevatorPosition { get; set; }
        public string? ElevatorDoorStatus { get; set; }
        public string? DeviceConnectionString { get; set; } = null;

    }
}
