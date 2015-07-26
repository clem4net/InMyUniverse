namespace Z4Net.Dto.Serial
{
    /// <summary>
    /// Header of the message.
    /// </summary>
    public enum MessageHeader
    {
        StartOfFrame = 0x01,
        Acknowledgment = 0x06,
        NotAcknowledgment = 0x15
    }
}
