using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using OtisElevatorDevice.Models;
using OtisElevatorDevice.Services;
using System.Drawing.Text;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace OtisElevatorDevice;


class Program
{
    DeviceManager deviceManager = new DeviceManager();
    public static List<ElevatorListItem> elevatorListItems = new List<ElevatorListItem>();


    public static void Main()
    {
        //kopplar eventuellt inte upp och lägger inte till items mot api! --fixa!
        //timer för att köra igenom alla hissar och uppdatera twins med nya data!

        //Task.Run(async ()=> await AddElevators());

        //var result = AddElevators().ConfigureAwait(false);

        Initialize();

        //await InitializeAsync();

        // foreach (var item in elevatorListItems)
        // {
        //     Console.WriteLine(item.Id.ToString());
        // }

        async void Initialize()
        {

            await AddElevators();
        }


        async Task <List<ElevatorListItem>> GetElevatorListItems()
        {

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
                        elevatorListItems.Add(item);
                    }
                }
            }
            catch { }
            return elevatorListItems.ToList();
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


                Console.WriteLine(await result.Content.ReadAsStringAsync());
            }
            catch { Console.WriteLine("Do not want!"); }

            
            Console.ReadKey();
        }

        async Task InitializeAsync()
        {

            var deviceList = await GetElevatorListItems();
            if (deviceList.Count == 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    await AddElevators();
                }

            }
            else
            {
                foreach (var item in deviceList)
                {
                    elevatorListItems.Add(item);
                }
            }
        }
    }
    


    
        
}











