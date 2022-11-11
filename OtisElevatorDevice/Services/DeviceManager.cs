using OtisElevatorDevice.Enums;
using OtisElevatorDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtisElevatorDevice.Services
{
    public class DeviceManager
    {
        List<ElevatorReturnData> returnData = new List<ElevatorReturnData>();

        public ElevatorReturnData GenerateData(ElevatorStates previousState, int topFloor, ElevatorListItem elevator)
        {

            ElevatorStates elevatorStatus = GenerateElevatorStatus(previousState);
            ElevatorReturnData returnData = new ElevatorReturnData
            {
                Id = elevator.Id,
                ElevatorStatus = elevatorStatus.ToString(),
                ElevatorPosition = GeneratePosition(topFloor, elevatorStatus).ToString(),

            };


            return returnData;
        }
        public ElevatorStates GenerateElevatorStatus(ElevatorStates previousStatus)
        {
            ElevatorStates status = previousStatus;
            if (status != ElevatorStates.Error && status != ElevatorStates.OutOfOrder)
            {
                Random random = new Random();
                int newRandom = random.Next(0, 100);
                if (newRandom <= 60)
                    status = ElevatorStates.StoppedOnFloor;
                else if(newRandom >= 61 && newRandom <= 97)
                    status = ElevatorStates.GoingToFloor;
                else if(newRandom == 99)
                    status = ElevatorStates.Error;
                else if(newRandom == 100)
                    status = ElevatorStates.OutOfOrder;
                //Array values = Enum.GetValues(typeof(ElevatorStates));
                //Random random2 = new Random();
                //ElevatorStates newStatus = (ElevatorStates)values.GetValue(random2.Next(values.Length));
                //if (!string.IsNullOrEmpty(newStatus.ToString()))
                //    status = newStatus;
            }

            return status;
        }

        


        public int GeneratePosition(int topFloor, ElevatorStates status)
        {
            int position = 0;
            if (status != ElevatorStates.Error || status != ElevatorStates.OutOfOrder)
            {
                Random random = new Random();

                position = random.Next(0, topFloor);

            }
                
            return position;
        }


        

        




    }
}
