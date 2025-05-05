// 4) https://www.hackerrank.com/challenges/birthday-cake-candles/problem?isFullScreen=true

public static int birthdayCakeCandles(List<int> candles)
    {
        // Find the tallest candle 
        int tallest = candles.Max();

        // Count how many times the tallest candle coms
        int tallestCount = candles.Count(c => c == tallest);

        return(tallestCount);
    }