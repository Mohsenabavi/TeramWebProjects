namespace Teram.HR.Module.Recruitment.ExtensionMethods
{
    public static class DateExtensions
    {
        public static string CalculateDaysDifference(DateTime createDate, DateTime? deadLineDate)
        {
            TimeSpan timeDifference = (deadLineDate.HasValue) ? deadLineDate.Value - createDate : TimeSpan.FromDays(2);
            int daysDifference = Math.Abs(timeDifference.Days)+1;
            return daysDifference switch
            {
                1 => "یک روز",
                2 => "دو روز",
                3 => "سه روز",
                4 => "چهار روز",
                5 => "پنج روز",
                6 => "شش روز",
                7 => "هفت روز",
                8 => "هشت روز",
                9 => "نه روز",
                10 => "ده روز",
                _ => $"{daysDifference} {"روز "}",
            };
        }
    }
}
