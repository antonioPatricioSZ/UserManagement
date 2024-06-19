namespace UserManagement.Domain.Entities;

public class BaseEntity {

    public DateTime CreationDate { get; set; } = 
        NormalizeDatetimeToSouthAmerica();


    private static DateTime NormalizeDatetimeToSouthAmerica() {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }

}