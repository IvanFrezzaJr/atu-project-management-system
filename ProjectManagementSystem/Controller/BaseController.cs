using ProjectManagementSystem.Core;
using System.Reflection;

namespace ProjectManagementSystem.Controller
{
    public class BaseController: Publisher
    {
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }

        protected void ValidateCondition(bool condition, string trueMessage, string falseMessage, Action<string> successCallback = null)
        {
            if (condition)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = trueMessage
                }, false);
                if (successCallback != null)
                {
                    successCallback(trueMessage);
                }
            }
            else
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = falseMessage
                }, false);
                throw new ApplicationException(falseMessage);
            }
        }

        protected void ValidateCondition(bool condition, string message)
        {
            if (condition == true)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = "Classroom already exists"
                }, false);
                throw new ApplicationException(message);
            }
        }
   
        protected void ValidateObjectInstance(object obj, string message = null, Action<string> successCallback = null)
        {
            if (obj == null)
            {
                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = message
                }, false);

                if (message != null && successCallback != null) {

                    successCallback(message);
                 }

                string throwMessage = "The object instance cannot be null.";

                if (message != null)
                    throwMessage = message;

                throw new ArgumentNullException(nameof(obj), throwMessage);
            }
               
        }

        protected void ValidateStringInput(string classroomName)
        {
            if (string.IsNullOrWhiteSpace(classroomName))
                throw new ArgumentException("Field is empty.");

            if (classroomName.Length < 3 || classroomName.Length > 50)
                throw new ArgumentException("String must be between 3 and 50 characters long.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(classroomName, @"^[a-zA-Z0-9\s]+$"))
                throw new ArgumentException("String can only contain letters, numbers and spaces.");
        }

        protected void ValidatePermission(string typeRole, string allowRole = "teacher")
        {
            if (allowRole != typeRole)
            {

                this.NotifyObservers(new Alert
                {
                    Role = this.GetType().Name,
                    Action = MethodBase.GetCurrentMethod().Name,
                    Message = $"Operation denied. Only allowed to assign '{typeRole}' role."
                }, false);

                throw new AccessViolationException($"Operation denied. Only allowed to assign '{typeRole}' role.");
            }
        }

        protected void ValidateFloatInput(float value)
        {
            // Check for overflow (float.MinValue and float.MaxValue are inherent bounds)
            if (value < float.MinValue || value > float.MaxValue)
                throw new OverflowException($"Value is out of range for a float: {value}");

            // If the value is infinity (positive or negative), it's an invalid float
            if (float.IsInfinity(value))
                throw new OverflowException("Value is infinite, which is not allowed.");

            // Check if the value is zero or negative
            if (value <= 0)
                throw new ArgumentException("Value must be a positive number.");
        }
    }

}
