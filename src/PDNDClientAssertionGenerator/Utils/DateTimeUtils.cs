// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
namespace PDNDClientAssertionGenerator.Utils
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// Convert a DateTime to a Unix timestamp
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int ToUnixTimestamp(this DateTime dateTime)
        {
            // Ensure the DateTime is in UTC to avoid timezone issues
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime.ToUniversalTime());

            // Convert to Unix time in seconds
            int unixTimestamp = (int)dateTimeOffset.ToUnixTimeSeconds();

            return unixTimestamp;
        }
    }
}
