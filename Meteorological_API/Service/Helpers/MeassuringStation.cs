namespace Meteorological_API.Service.Helpers
{
    /// <summary>
    /// Represents a measuring station and provides utility methods for working with station data.
    /// </summary>
    public class MeassuringStation
    {
        /// <summary>
        /// Get URL syntax augmentation for a given station key.
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        public static string GetValueUrlAugmentationOrEmpty(long? stationKey)
        {
            return stationKey == null || stationKey == 0 ? string.Empty : $"/station/{stationKey}";
        }
    }
}
