namespace RpiStats.Models
{
    public enum ThrottledState
    {
        UnderVoltageDetected = 0,
        FrequencyCapped = 1,
        CurrentlyThrottled = 2,
        SoftTemperatureLimit = 3,
        UnderVoltHasOccuredSinceBoot = 16,
        FrequencyCapHasOccured = 17,
        ThrottlingHasOccured = 18,
        SoftTemperatureLimitHasOccured = 19
    }
}
