namespace MyHostAPI.Models.Premise
{
    public class PremiseWorkHoursModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int OpeningTimeInMinutes { get; set; }
        public int ClosingTimeInMinutes { get; set; }
    }
}
