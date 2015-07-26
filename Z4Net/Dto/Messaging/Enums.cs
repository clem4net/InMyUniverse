using System;

namespace Z4Net.Dto.Messaging
{

    /// <summary>
    /// Command of the message.
    /// </summary>
    public enum MessageCommand
    {
        /// <summary>
        /// Get nodes ids known by a controller.
        /// </summary>
        GetControllerNodes = 0x02,
        /// <summary>
        /// Request application command handler
        /// </summary>
        NodeValueChanged = 0x04,
        /// <summary>
        /// Ask controller capacities.
        /// </summary>
        GetControllerCapabilities = 0x05,
        /// <summary>
        /// Ask serial port capacities.
        /// </summary>
        GetApiCapabilities = 0x07,
        /// <summary>
        /// Send data to node.
        /// </summary>
        SendData = 0x13,
        /// <summary>
        /// Ask Z version
        /// </summary>
        GetVersion = 0x15,
        /// <summary>
        /// Ask home identifier.
        /// </summary>
        GetHomeId = 0x20,
        /// <summary>
        /// Get node protocol (class type ...)
        /// </summary>
        GetNodeProtocol = 0x041,
        /// <summary>
        /// Request of supported classes by controller.
        /// </summary>
        ApplicationUpdate = 0x49,
        /// <summary>
        /// Ask the SUC mode controller information.
        /// </summary>
        GetSucMode = 0x56
    }

    /// <summary>
    /// Type of the message.
    /// </summary>
    public enum MessageType
    {
        Request = 0x00,
        Response = 0x01
    }


    /// <summary>
    /// Transmission option when data is set.
    /// </summary>
    [Flags]
    public enum TransmitOptions
    {
        Acknowlegment = 0x01,
        LowPower = 0x02,
        AutoRoute = 0x04,
        NoRoute = 0x10,
        Explore = 0x20
    }

    /// <summary>
    /// States of send queue.
    /// </summary>
    public enum QueueState
    {
        /// <summary>
        /// Take next message.
        /// </summary>
        Process = 0,
        /// <summary>
        /// Wait for end of proces of last sent message.
        /// </summary>
        Wait = 1,
        /// <summary>
        /// No more message, stop the queue.
        /// </summary>
        Stop = 2
    }

}
