namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class PrincipalView : BaseView
    {
        public void ShowSubmissionsResult(List<dynamic> submissions)
        {
            foreach (var submission in submissions)
            {
                Console.WriteLine($"Student: {submission.StudentName}");
                Console.WriteLine($"Assessment: {submission.AssessmentDescription}");
                Console.WriteLine($"Score: {submission.ScoreStatus} / {submission.MaxScore}");
                Console.WriteLine(new string('-', 30)); // Separator line
            }
        }
       
    }
}

