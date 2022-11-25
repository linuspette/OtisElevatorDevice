﻿namespace OtisElevatorDevice.Models;

public class ElevatorDataPayload
{
    public string DeviceId { get; set; } = null!;
    public string DeviceName { get; set; } = null!;
    public string DeviceType { get; set; } = null!;
    public string Elevatorstatus { get; set; } = null!;
    public string Elevatorposition { get; set; } = null!;
    public string Elevatordoorstatus { get; set; } = null!;
    public DateTime LocalTimeStamp { get; set; } = DateTime.Now.ToLocalTime();
}