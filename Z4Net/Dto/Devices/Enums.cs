﻿using Z4Net.Business.Devices;
using Z4Net.Dto.Attributes;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Command class to process.
    /// </summary>
    public enum CommandClass
    {
        None = 0x00,
        [DataReceived(typeof(SwitchBinaryBusiness))]
        SwitchBinaryAction = 0x25,
        Configuration = 0x70
    }

    /// <summary>
    /// Device type.
    /// </summary>
    public enum DeviceClass
    {
        Unknown = 0x00,
        StaticControler = 0x01,
        Controler = 0x02,
        EnhancedSlave = 0x03,
        Slave = 0x04,
        Installer = 0x05,
        RoutingSlave = 0x06,
        BridgeControler = 0x07,
        DeviceUnderTest = 0x08
    }

    /// <summary>
    /// Generic class type.
    /// </summary>
    public enum DeviceClassGeneric
    {
        Other = 0x00,
        [DataReceived(typeof(ControlerBusiness))]
        StaticControler = 0x02,
        AvControlPoiunt = 0x03,
        Display = 0x04,
        Thermostat = 0x08,
        [DataReceived(typeof(SwitchBinaryBusiness))]
        SwitchBinary = 0x10,
        SwitchMultilevel = 0x11,
        SwitchRemote = 0x12,
        SwitchToggle = 0x13,
        Ventilation = 0x16,
        SensorBinary = 0x20,
        SensorMultilevel = 0x21,
        MeterPulse = 0x30,
        Meter = 0x31,
        EentyControl = 0x40,
        AlarmSensor = 0xa1
    }

    /// <summary>
    /// Switch binary actions.
    /// </summary>
    public enum SwitchBinaryAction
    {
        Set = 1,
        Get = 2,
        Report = 3
    }

}
