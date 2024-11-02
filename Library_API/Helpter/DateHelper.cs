// Helpers/DateHelper.cs
using System;

namespace Library_API.Helpers
{
    public static class DateHelper
    {
        // Hàm tính số giờ làm việc giữa hai ngày
        public static int CalculateWorkingHours(DateTime startDate, DateTime endDate)
        {
            int workingHours = 0;
            DateTime current = startDate;

            while (current < endDate)
            {
                // Bỏ qua thứ Bảy và Chủ Nhật
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingHours++;
                }
                current = current.AddHours(1);
            }

            return workingHours;
        }
    }
}
