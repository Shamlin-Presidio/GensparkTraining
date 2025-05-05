// 6) https://www.hackerrank.com/challenges/grading/problem?isFullScreen=true

    public static List<int> gradingStudents(List<int> grades)
    {
        // Get the rounded grades
        List<int> roundedGrades = RoundGrades(grades);
        return roundedGrades;
    }
    
    // Round the grades
    static List<int> RoundGrades(List<int> grades)
    {
        List<int> roundedGrades = new List<int>();
        foreach (int grade in grades)
        {
            if (grade >= 38)
            {
                // Find the next multiple of 5
                int nextMultipleOf5 = ((grade / 5) + 1) * 5;
                // If the difference is less than 3, round up
                if (nextMultipleOf5 - grade < 3)
                {
                    roundedGrades.Add(nextMultipleOf5);
                }
                else
                {
                    roundedGrades.Add(grade);
                }
            }
            else
            {
                // No rounding if the grade is less than 38
                roundedGrades.Add(grade);
            }
        }
        return roundedGrades;
    }
