using ProjectManagementSystem.Models;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;

namespace ProjectManagementSystem.Views
{
    // Class responsible for handling user input/output
    public class StaffView : BaseView
    {
        public void DisplayClassroomInfo(Classroom classroom)
        {
            Console.WriteLine($"ClassName: {classroom.Name}");
        }

    }
}

