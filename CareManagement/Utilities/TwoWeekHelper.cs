namespace CareManagement.Utilities
{
    public class TwoWeekHelper
    {
        public static List<string> GetTwoWeekPeriods()
        {
            var currentDate = DateTime.Today;
            var janFirst = new DateTime(currentDate.Year, 1, 1);
            var startDate = janFirst;
            var endDate = startDate.AddDays(13);

            var result = new List<string>();
            while (endDate <= currentDate)
            {
                result.Add($"{startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}");

                startDate = endDate.AddDays(1);
                endDate = startDate.AddDays(13);

                if (endDate > currentDate && startDate <= currentDate)
                {
                    result.Add($"{startDate:yyyy-MM-dd} - {endDate:yyyy-MM-dd}");
                }
            }

            result.Reverse();

            return result;
        }

    }
}
