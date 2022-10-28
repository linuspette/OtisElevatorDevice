using DotNetty.Common.Utilities;
using Microsoft.Azure.Cosmos.Spatial;
using OtisElevatorDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static OtisElevatorDevice.Services.ReturnDataService;

namespace OtisElevatorDevice.Services
{
    public interface IReturnDataService
    {
        public ElevatorReturnData GenerateData(ElevatorStates previousState, int previousPosition, int topFloor);
        public ElevatorStates GenerateElevatorStatus(ElevatorStates previousStatus);
        public int GeneratePosition(int topFloor, int previousPosition, ElevatorStates status);


    }

    public class ReturnDataService : IReturnDataService
    {
        public ElevatorReturnData GenerateData(ElevatorStates previousState, int previousPosition, int topFloor)
        {

            ElevatorStates elevatorStatus = GenerateElevatorStatus(previousState);
            ElevatorReturnData returnData = new ElevatorReturnData
            {
                Id = 0,
                ElevatorStatus = elevatorStatus.ToString(),
                ElevatorPosition = GeneratePosition(topFloor, previousPosition, elevatorStatus).ToString(),
                
            };


            return returnData;
        }
         public ElevatorStates GenerateElevatorStatus(ElevatorStates previousStatus)
        {
            ElevatorStates status = previousStatus;
            if(status != ElevatorStates.Error || status != ElevatorStates.OutOfOrder)
            {
                Array values = Enum.GetValues(typeof(ElevatorStates));
                Random random = new Random();
                ElevatorStates newStatus = (ElevatorStates)values.GetValue(random.Next(values.Length));
                if(!string.IsNullOrEmpty(newStatus.ToString()))
                    status = newStatus;
            }

            return status;
        }
        


        public int GeneratePosition(int topFloor, int previousPosition, ElevatorStates status)
        {
            int position = previousPosition;
            if (status != ElevatorStates.Error || status != ElevatorStates.OutOfOrder)
                position = new Random().Next(0,topFloor);
            
            return position;
        }


        public enum ElevatorStates
        {
            Error,
            OutOfOrder,
            StoppedOnFloor,
            GoingToFloor

        }

        public enum DoorStatus
        {
            Open,
            Closed,
            Error
        }

    }
}
