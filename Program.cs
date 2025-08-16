using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Tracker_Weekly_Report
{
    public class UserFitness
    {
        public string Name { get; }
        public int[] DailySteps { get; }
        public int[] DailyCalories { get; }
        public int StepGoal { get; }
        public int CalorieGoal { get; }

        public UserFitness(string name, int[] dailySteps, int[] dailyCalories, int stepGoal, int calorieGoal)
        {
            if (dailySteps == null || dailyCalories == null)
                throw new ArgumentNullException("Steps and calories data cannot be null");

            if (dailySteps.Length != 7 || dailyCalories.Length != 7)
                throw new ArgumentException("Must provide exactly 7 days of data");

            if (dailySteps.Any(s => s < 0) || dailyCalories.Any(c => c < 0))
                throw new ArgumentException("Values cannot be negative");

            if (stepGoal < 0 || calorieGoal < 0)
                throw new ArgumentException("Goals cannot be negative");

            Name = name;
            DailySteps = dailySteps;
            DailyCalories = dailyCalories;
            StepGoal = stepGoal;
            CalorieGoal = calorieGoal;
        }

        public (double steps, double calories) Average()
        {
            return (DailySteps.Average(), DailyCalories.Average());
        }

        public int BestDay()
        {
            int bestDay = 0;
            double bestScore = 0;

            for (int i = 0; i < 7; i++)
            {
                double stepAchievement = (double)DailySteps[i] / StepGoal;
                double calorieAchievement = (double)DailyCalories[i] / CalorieGoal;
                double dayScore = stepAchievement + calorieAchievement;

                if (dayScore > bestScore)
                {
                    bestScore = dayScore;
                    bestDay = i;
                }
            }

            return bestDay + 1; 
        }

        public bool OnTrack()
        {
            int successfulDays = 0;
            for (int i = 0; i < 7; i++)
            {
                if (DailySteps[i] >= StepGoal && DailyCalories[i] >= CalorieGoal)
                {
                    successfulDays++;
                }
            }
            return successfulDays >= 5;
        }

        public void DisplayReport()
        {
            var averages = Average();
            int bestDay = BestDay();
            bool onTrack = OnTrack();

            Console.WriteLine($"\nFITNESS REPORT FOR {Name.ToUpper()}");
            Console.WriteLine($"Step Goal: {StepGoal} | Calorie Goal: {CalorieGoal}");
            Console.WriteLine("\nDaily Breakdown:");
            for (int i = 0; i < 7; i++)
            {
                string stepsStatus = DailySteps[i] >= StepGoal ? "[✓]" : "[✗]";
                string caloriesStatus = DailyCalories[i] >= CalorieGoal ? "[✓]" : "[✗]";
                Console.WriteLine($"Day {i + 1}: Steps: {DailySteps[i]}{stepsStatus} | Calories: {DailyCalories[i]}{caloriesStatus}");
            }

            Console.WriteLine($"\n7-Day Averages: Steps: {averages.steps:F0}, Calories: {averages.calories:F0}");
            Console.WriteLine($"Best Performance Day: Day {bestDay}");
            Console.WriteLine($"On Track (≥5 good days): {(onTrack ? "YES" : "NO")}");
        }
    }
    public class FitnessTracker
    {
        private List<UserFitness> users = new List<UserFitness>();

        public void AddUser(UserFitness user)
        {
            users.Add(user);
        }

        public UserFitness GetUserByName(string name)
        {
            return users.FirstOrDefault(u =>
                u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void GenerateUserReport(string name)
        {
            var user = GetUserByName(name);
            if (user == null)
            {
                Console.WriteLine($"User '{name}' not found.");
                return;
            }
            user.DisplayReport();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var tracker = new FitnessTracker();

                tracker.AddUser(new UserFitness("Alice",
                    new int[] { 8520, 10023, 7550, 12010, 9325, 11000, 6500 },
                    new int[] { 1850, 2100, 1750, 2250, 1950, 2050, 1600 },
                    8000, 2000));

                tracker.AddUser(new UserFitness("Bob",
                    new int[] { 5000, 6025, 7200, 6500, 7000, 5500, 8000 },
                    new int[] { 2200, 2400, 2100, 2300, 2150, 2250, 2350 },
                    7500, 2500));

                tracker.GenerateUserReport("Alice");
                tracker.GenerateUserReport("Bob");
                tracker.GenerateUserReport("Charlie"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
