using System.Globalization;

namespace UploadStreamToQuestDB.Domain.Utilities {
    /// <summary>
    /// Utility class for date and time conversions.
    /// </summary>
    public static class DateTimeUtility {
        /// <summary>
        /// Converts a string in "yyyyMMddHHmm" format to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="input">The date string in "yyyyMMddHHmm" format.</param>
        /// <returns>The parsed <see cref="DateTime"/> object.</returns>
        public static DateTime yyyyMMddHHmmToDate(string input) {
            DateTime parsedDate = DateTime.ParseExact(input,
                                  "yyyyMMddHHmm",
                                  CultureInfo.InvariantCulture);
            return parsedDate;
        }

        /// <summary>
        /// Converts a string in "yyyyMMddHHmm" format to a QuestDB date string in "yyyy-MM-ddTHH:mm:ss.ffffffZ" format.
        /// </summary>
        /// <param name="input">The date string in "yyyyMMddHHmm" format.</param>
        /// <returns>The formatted QuestDB date string.</returns>
        public static string DateToQuestDbDateString(string input) {
            DateTime parsedDate = DateTime.ParseExact(input,
                                  "yyyyMMddHHmm",
                                  CultureInfo.InvariantCulture);
            var res = parsedDate.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
            return res;
        }
    }
}
