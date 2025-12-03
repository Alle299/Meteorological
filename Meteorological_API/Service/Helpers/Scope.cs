namespace Meteorological_API.Service.Helpers
{
    public class Scope
    {
        /// <summary>
        /// Get URL syntax augmentation for a given latestDay bool.
        /// </summary>
        /// <param name="latestDay"></param>
        /// <returns></returns>
        public static string GetValueUrlAugmentationOrEmpty(bool? latestDay)
        {
            return latestDay == true ? "latest-day" : "latest-hour";
        }
    }
}
