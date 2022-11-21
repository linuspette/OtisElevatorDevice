using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using OtisElevatorDevice.Enums;
using OtisElevatorDevice.Models;
using System.Net.Http.Json;
using System.Text;

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

            try
            {
                using var http = new HttpClient();

                IEnumerable<ElevatorListItem> returnList = await http.GetFromJsonAsync<IEnumerable<ElevatorListItem>>("https://otisagileapi.azurewebsites.net/api/Elevators/getelevators/?take=10");

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
            if (elevatorListItems.Count < 20)
            {
                int count = 20 - elevatorListItems.Count;
                for (int i = 0; i < count; i++)
                {
                    await AddElevators();
                }
            }
            await Task.Delay(2000);
            await UpdateElevatorTwinsAsync();
            await SendDataAsync();
        }

        async Task UpdateElevatorTwinsAsync()
        {

            if (elevatorListItems.Count != 0)
            {
                foreach (var item in elevatorListItems)
                {
                    if (item != null)
                    {
                        using var http = new HttpClient();

                        var deviceConnectionString = await http.PostAsJsonAsync("https://otisfunctions.azurewebsites.net/api/devices/connect", new { deviceId = item.Id });
                        var result = await deviceConnectionString.Content.ReadAsStringAsync();
                        item.DeviceConnectionString = result.ToString();
                        Random random = new Random();
                        int topFloor = random.Next(0, 10);
                        ElevatorReturnData returnUpdate = new ElevatorReturnData();
                        returnUpdate = deviceManager.GenerateData(ElevatorStates.GoingToFloor, topFloor, item);

                        if (item.DeviceConnectionString != null)
                        {
                            await using var _deviceClient = DeviceClient.CreateFromConnectionString(item.DeviceConnectionString);

                            var twin = await _deviceClient.GetTwinAsync();
                            if (twin != null)
                            {
                                TwinCollection reported = new TwinCollection();

                                reported["ElevatorStatus"] = returnUpdate.ElevatorStatus;
                                reported["ElevatorPosition"] = returnUpdate.ElevatorPosition;
                                reported["ElevatorDoorStatus"] = returnUpdate.ElevatorDoorStatus;

                                await _deviceClient.UpdateReportedPropertiesAsync(reported);
                            }
                            Console.WriteLine(returnUpdate.ElevatorStatus.ToString());

                        }
                    };
                }
            }
        }

        async Task SendDataAsync()
        {
            while (true)
            {
                foreach (var elevator in elevatorListItems)
                {
                    if (elevator.DeviceConnectionString != null)
                    {
                        try
                        {
                            var msg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ElevatorDataPayload
                            {
                                DeviceId = elevator.Id!,
                                DeviceName = "Otis Elevator 199",
                                DeviceType = "Small elevator",
                            })));

                            await SendMessageAsync(elevator.DeviceConnectionString!, msg);
                        }
                        catch { }
                    }
                }
                await Task.Delay(5000);
            }
        }

        public async Task SendMessageAsync(string connectionString, Message data)
        {
            await using var _deviceClient = DeviceClient.CreateFromConnectionString(connectionString);
            await _deviceClient.SendEventAsync(data);
        }



    }
}
