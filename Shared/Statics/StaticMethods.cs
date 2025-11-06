using Shared.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Shared.Statics
{
    public static class StaticMethods
    {
        public static bool IsCrudOperationSuccessful(int num)
        {
            return num > 0;
        }
        public static ObservableCollection<int> UpdateDaysInMonth(int year,Months month)
        {
            try
            {
                int daysInMonth = GetDaysInMonth(month, year);
                var localList = Enumerable.Range(1, daysInMonth).ToList();
                return new ObservableCollection<int>(localList);
            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }

        }

        public static int GetDaysInMonth(Months month, int year)
        {
            switch (month)
            {
                case Months.February:
                    return DateTime.IsLeapYear(year) ? 29 : 28;
                case Months.April:
                case Months.June:
                case Months.September:
                case Months.November:
                    return 30;
                default:
                    return 31;
            }
        }
        public static bool IsValidISBN13(long isbn)
        {
            string TestISBN = isbn.ToString();
            if (TestISBN.Length != 13 || !TestISBN.All(char.IsDigit)) return false;

            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += (i % 2 == 0 ? 1 : 3) * (TestISBN[i] - '0');

            int check = (10 - (sum % 10)) % 10;
            return check == (TestISBN[12] - '0');
        }

        public static DateOnly DateCreeator(int year, Months month, int day)
        {
            try
            {
                return new DateOnly(year, (int)month, day);
            }
            catch
            {
                return DateOnly.MinValue; // invalid date
            }
        }

        public static bool ShowYesNoWarning(string message, string caption = "Warning")
        {
            var result = MessageBox.Show(
                message,
                caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.Yes
            );

            return result == MessageBoxResult.Yes;
        }
        public static void ShowError(string message = StaticConstants.FILL_ALL_FIELDS)
        {
            MessageBox.Show(message, StaticConstants.ERROR, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

    }
}

