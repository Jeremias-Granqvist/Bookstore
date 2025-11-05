using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Model;

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

    }
}
