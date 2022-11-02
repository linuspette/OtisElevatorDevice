using OtisElevatorDevice.Enums;
using OtisElevatorDevice.Models;
using OtisElevatorDevice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ElevatorDeviceTests.Services
{

    public class ReturnDataServiceTests
    {
        private DeviceManager _sut;
        public ReturnDataServiceTests()
        {
            _sut = new DeviceManager();
        }

        [Fact]
        public void PassingTest_CreateElevatorReturnData()
        {
            int topFloor = 5;
            var status = ElevatorStates.GoingToFloor;

            var result = _sut.GenerateData(status, topFloor);

            Assert.IsType<ElevatorReturnData>(result);
        }



        [Fact]
        public void PassingTest_CreateNewPosition()
        {
            int topFloor = 5;
            var status = ElevatorStates.GoingToFloor;
            var result = _sut.GeneratePosition(topFloor,  status);
            //Assert.IsType<int>(result);
            Assert.InRange<int>(result, 0, topFloor); 
        }

        [Fact]
        public void PassingTest_GeneratesNewElevatorState()
        {
            var previousState = ElevatorStates.GoingToFloor;
            var result = _sut.GenerateElevatorStatus(previousState);
            Assert.IsType<ElevatorStates>(result);
        }

        [Fact]
        public void FailingTest_GeneratesNewElevatorStateBecausePreviousIsError()
        {
            var previousState = ElevatorStates.Error;
            ElevatorStates result = ElevatorStates.GoingToFloor;

            for (int i = 0; i < 50; i++)
            {
                result = _sut.GenerateElevatorStatus(previousState);
            }


            Assert.Equal(ElevatorStates.Error, result);
        }

    }
}
