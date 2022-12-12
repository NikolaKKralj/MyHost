namespace MyHostAPI.Models.Premise
{
    public class PremiseTimeSettingsModel
    {
        public int MinStartTimeFromNowInMinutes { get; set; }
        public int MinDurationInMinutes { get; set; }
        public int MaxDurationInMinutes { get; set; }
        public int DefaultDurationInMinutes { get; set; }
        public List<PremiseWorkHoursModel> WorkHours { get; set; } = null!;
        public int MinutInterval { get; set; }
    }
}
