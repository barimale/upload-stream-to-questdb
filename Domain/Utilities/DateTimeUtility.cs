using System.Globalization;

namespace Domain.Utilities {
    public static class DateTimeUtility {
        public static DateTime yyyyMMddHHmmToDate(string input) {
            DateTime parsedDate = DateTime.ParseExact(input,
                                  "yyyyMMddHHmm",
                                  CultureInfo.InvariantCulture);
            return parsedDate;
        }

        public static string DateToQuestDbDateString(string input) {
            DateTime parsedDate = DateTime.ParseExact(input,
                                  "yyyyMMddHHmm",
                                  CultureInfo.InvariantCulture);
            var res = parsedDate.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
            return res;
        }
    }
}
