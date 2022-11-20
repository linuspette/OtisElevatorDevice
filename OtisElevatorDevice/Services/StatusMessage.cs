using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace OtisElevatorDevice.Services
{
    public class ElevatorStatusMessage
    { 
        public string? StatusMessage { get; set; }

        async Task Checkstatus()
        {
            var twinCollection = new TwinCollection();
            while (true)
            {

            }
        }

        private async Task GenerateMessage(string message)
        {
         
        }
        private async Task Checkmessage(string message)
        {
            if (message == null)
            {
                return;
            }
            else
            {

            }
        }

        public static string Sendmessage(
            string message,
            CancellationToken cancellationToken)
        {
            if(message == null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
            else
            {
                return message;
            }
            
        }



    }
}
