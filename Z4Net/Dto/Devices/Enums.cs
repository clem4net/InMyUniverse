using Z4Net.Business.Devices;
using Z4Net.Dto.Attributes;

namespace Z4Net.Dto.Devices
{
    /// <summary>
    /// Command class to process.
    /// </summary>
    public enum RequestCommandClass
    {
        None = 0x00,
        /// <summary>
        /// Process a switch binary.
        /// </summary>
        [DataReceived(typeof(SwitchBinaryBusiness))]
        SwitchBinaryAction = 0x25,
        /// <summary>
        /// Configure a node.
        /// </summary>
        [DataReceived(typeof(ConfigurationBusiness))]
        Configuration = 0x70,
        /// <summary>
        /// Get node constructor, product identifier ...
        /// </summary>
        [DataReceived(typeof(ConstructorBusiness))]
        Constructor = 0x72
    }

    /// <summary>
    /// Device type.
    /// </summary>
    public enum DeviceClass
    {
        Unknown = 0x00,
        StaticController = 0x01,
        Controller = 0x02,
        EnhancedSlave = 0x03,
        Slave = 0x04,
        Installer = 0x05,
        RoutingSlave = 0x06,
        BridgeController = 0x07,
        DeviceUnderTest = 0x08
    }

    /// <summary>
    /// Generic class type.
    /// </summary>
    public enum DeviceClassGeneric
    {
        Other = 0x00,
        [DataReceived(typeof(ControllerBusiness))]
        StaticController = 0x02,
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

    /// <summary>
    /// Configuration action.
    /// </summary>
    public enum ConfigurationAction
    {
        Set = 0x04,
        Get = 0x05,
        Report = 0x06
    };

    /// <summary>
    /// Constructor action.
    /// </summary>
    public enum ConstructorAction
    {
        Get = 0x04,
        Report = 0x05
    }

}
