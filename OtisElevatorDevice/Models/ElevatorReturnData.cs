using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtisElevatorDevice.Models
{
    public class ElevatorReturnData
    {
        public Guid Id { get; set; } = new Guid();
        public string? ElevatorStatus { get; set; }
        public string? ElevatorPosition { get; set; }
        public string? ElevatorDoorStatus { get; set; }

        //ERRROR
        
    }
}
