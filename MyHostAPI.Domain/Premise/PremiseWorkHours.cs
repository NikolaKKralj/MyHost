namespace MyHostAPI.Domain.Premise
{
    public class PremiseWorkHours
    {
        public DayOfWeek DayOfWeek { get; set; }
        // from midnight
        public int OpeningTimeInMinutes { get; set; } 
        public int ClosingTimeInMinutes { get; set; }
    }
}
