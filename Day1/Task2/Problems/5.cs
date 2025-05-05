// 5) https://www.hackerrank.com/challenges/time-conversion/problem?isFullScreen=true

public static string timeConversion(string s)
    {
        string period = s.Substring(8, 2);  // AM or PM
        int hour = int.Parse(s.Substring(0, 2));  // hours 
        string minutes = s.Substring(3, 2);  // minutes
        string seconds = s.Substring(6, 2);  // seconds 

        // Convert to military time
        if (period == "AM")
        {
            if (hour == 12)
            {
                hour = 0;  // 12 AM is 00 in military time
            }
        }
        else 
        {
            if (hour != 12)
            {
                hour += 12; 
            }
        }
        // Format the hour to two digits (e.g., "01", "12")
        string militaryHour = hour.ToString("D2");
        // Return the military time in HH:MM:SS format
        return $"{militaryHour}:{minutes}:{seconds}";
    }