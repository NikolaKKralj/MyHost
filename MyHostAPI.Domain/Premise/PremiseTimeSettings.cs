namespace MyHostAPI.Domain.Premise
{
    public class PremiseTimeSettings
    {
        /// <summary>
        /// Information about Premise Reservations
        /// </summary>
        public int MinStartTimeFromNowInMinutes { get; set; } 
        public int MinDurationInMinutes { get; set; }
        public int MaxDurationInMinutes { get; set; }
        public int DefaultDurationInMinutes { get; set; }
        /// <summary>
        /// Information about Premise Work Hours
        /// </summary>
        public List<PremiseWorkHours> WorkHours{ get; set; } = null!;
        public int MinutInterval { get; set; }
    }
}
