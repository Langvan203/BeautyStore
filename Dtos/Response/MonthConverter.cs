using System.Globalization;

namespace my_cosmetic_store.Dtos.Response
{
    public class MonthConverter
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public static List<MonthConverter> Months { get; } = CreateMonthList();

        private static List<MonthConverter> CreateMonthList()
        {
            List<MonthConverter> months = new List<MonthConverter>();
            for (int i = 1; i <= 12; i++)
            {
                months.Add(new MonthConverter
                {
                    Month = i,
                    MonthName = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3)
                });
            }
            return months;
        }
    }
}
