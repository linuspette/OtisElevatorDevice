using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using OtisElevatorDevice.Enums;
using OtisElevatorDevice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OtisElevatorDevice.Services
{
    public class ElevatorInitialize
    {
        private List<ElevatorListItem> elevatorListItems = new List<ElevatorListItem>();
        private readonly DeviceManager deviceManager = new DeviceManager();
        public void Initialize()
        {
            InitializeAsync().ConfigureAwait(false);

        }

        async Task<List<ElevatorListItem>> GetElevatorListItems()
        {
            List<ElevatorListItem> elevatorList = new List<ElevatorListItem>();

            List<ElevatorListItem> elevatorListItems = new List<ElevatorListItem>();
            try
            {
                using var http = new HttpClient();

                var result = await http.GetAsync("https://otisagileapi.azurewebsites.net/api/Elevators/getelevators/100");
                IEnumerable<ElevatorListItem> returnList = (IEnumerable<ElevatorListItem>)JsonConvert.DeserializeObject<ElevatorListItem>(await result.Content.ReadAsStringAsync());
                if (returnList != null)
                {
                    foreach (var item in returnList)
                    {
                        elevatorList.Add(item);
                    }
                }
            }
            catch { }
            return elevatorList.ToList();
        }


        async Task AddElevators()
        {

            var client = new HttpClient();

            client.BaseAddress = new Uri("https://otisagileapi.azurewebsites.net/api/");
            try
            {
                var result = await client.PostAsJsonAsync("elevators/add", new
                {

                    id = Guid.NewGuid().ToString(),
                    location = "IN THE SHAFT"


                });

            }
            catch { Console.WriteLine("Do not want!"); }


            Console.ReadKey();
        }

        async Task InitializeAsync()
        {

            elevatorListItems = await GetElevatorListItems();
            if (elevatorListItems.Count < 10)
            {
                int count = 10 - elevatorListItems.Count;
                for (int i = 0; i < count; i++)
                {
                    await AddElevators();
                }

            }
            
        }

        async Task UpdateElevatorTwins()
        {
            if(elevatorListItems.Count != 0)
            {
                foreach (var item in elevatorListItems)
                {
                    if(item != null)
                    {
                    Random random = new Random();
                    int topFloor = random.Next(0, 10);
                    ElevatorReturnData returnUpdate = new ElevatorReturnData();                   
                    returnUpdate = deviceManager.GenerateData(ElevatorStates.GoingToFloor, topFloor, item);

                    if(item.DeviceConnectionString != null)
                        {
                            using var _deviceClient = DeviceClient.CreateFromConnectionString(item.DeviceConnectionString);

                            var twin = await _deviceClient.GetTwinAsync();
                            if (twin != null)
                            {
                                TwinCollection reported = new TwinCollection();

                                reported["ElevatorStatus"] = returnUpdate.ElevatorStatus;
                                reported["ElevatorPosition"] = returnUpdate.ElevatorPosition;
                                reported["ElevatorDoorStatus"] = returnUpdate.ElevatorDoorStatus;

                                await _deviceClient.UpdateReportedPropertiesAsync(reported);
                            }

                        }
                    


                    };
                    

                }
            }

        }

        public async Task LoopUpdates()
        {
            while(elevatorListItems.Count != 0) 
            {
                await UpdateElevatorTwins();
                Task.Delay(20000);
            
            }

        }



    }
}
