using ProjectManagementSystem;

/* how to subscribe a object
 
    // Create instances of students and an admin
    Student ivan = new Student("Ivan");
    Student jose = new Student("Jose");
    Admin admin = new Admin("Super admin");

    // Subscribe the admin to both students
    ivan.AddSubscriber(admin);
    jose.AddSubscriber(admin);

    // Students submit events
    ivan.Submit("Hello");
    jose.Submit("World");

    // Admin prints the logs of the events it received
    admin.PrintLogs();

 */

namespace ProjectManagementSystem
{
    /// <summary>
    /// Represents an event that can be sent to observers.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The role associated with the event. E.g. Student, Teacher
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// The action related to the event. It must be passed to the class methods.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The message or description of the event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The timestamp when the event was created.
        /// </summary>
        public DateTime CreatedAt { get; } = DateTime.Now;

        public Event(Role role, string action, string message)
        {
            Role = role;
            Action = action;
            Message = message;
        }
    }

    /// <summary>
    /// Defines the contract for a subscriber, which can update on an event.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Updates the subscriber with the given event.
        /// </summary>
        /// <param name="event">The event to update the subscriber with.</param>
        void Update(Event @event);
    }

    /// <summary>
    /// Defines the contract for a publisher, which can add subscribers and notify them.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Adds a subscriber to the publisher.
        /// </summary>
        /// <param name="subscriber">The subscriber to add.</param>
        void AddSubscriber(ISubscriber subscriber);

        /// <summary>
        /// Notifies all subscribers about an event.
        /// </summary>
        /// <param name="event">The event to notify subscribers with.</param>
        void NotifyObservers(Event @event);
    }

    /// <summary>
    /// Abstract class representing a subscriber which receives events.
    /// </summary>
    public abstract class Subscriber : ISubscriber
    {
        /// <summary>
        /// Abstract method that must be implemented by derived classes to update the subscriber with the event.
        /// </summary>
        /// <param name="event">The event to update the subscriber with.</param>
        public abstract void Update(Event @event);
    }

    /// <summary>
    /// Abstract class representing a publisher that can manage subscribers and notify them.
    /// </summary>
    public abstract class Publisher : IPublisher
    {
        /// <summary>
        /// List of subscribers to be notified.
        /// </summary>
        private List<ISubscriber> _subscribers = new List<ISubscriber>();

        /// <summary>
        /// Adds a subscriber to the publisher's subscriber list.
        /// </summary>
        /// <param name="subscriber">The subscriber to add.</param>
        public void AddSubscriber(ISubscriber subscriber)
        {
            // Add subscriber if not already present
            if (!this._subscribers.Contains(subscriber))
            {
                this._subscribers.Add(subscriber);
            }
        }

        /// <summary>
        /// Notifies all subscribers about the event.
        /// </summary>
        /// <param name="event">The event to notify the subscribers about.</param>
        public void NotifyObservers(Event @event)
        {
            // Notify each subscriber with the event
            foreach (var subscriber in this._subscribers)
            {
                subscriber.Update(@event);
            }
        }
    }

    /// <summary>
    /// Represents a role, which is also a publisher that can notify its subscribers.
    /// </summary>
    public class Role : Publisher
    {
        /// <summary>
        /// The name of the role.
        /// </summary>
        public string Name { get; set; }

        public Role(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Represents a student, which is a type of role and can submit events.
    /// </summary>
    public class Student : Role
    {
        public Student(string name) : base(name) { }

        /// <summary>
        /// Submits an event with the given message, notifying subscribers.
        /// </summary>
        /// <param name="message">The message of the event being submitted.</param>
        public void Submit(string message)
        {
            System.Console.WriteLine($"{Name} is submitting: {message}");

            // Create an event and notify observers
            Event @event = new Event(this, "Submit", message);
            this.NotifyObservers(@event);
        }
    }

    /// <summary>
    /// Represents an admin subscriber, who can receive and log events.
    /// </summary>
    public class Admin : Subscriber
    {
        /// <summary>
        /// The role of the admin.
        /// </summary>
        private string _role = string.Empty;

        /// <summary>
        /// A list of logs containing events received by the admin.
        /// </summary>
        private List<Event> _logs = new List<Event>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Admin"/> class with the specified role.
        /// </summary>
        /// <param name="role">The role of the admin.</param>
        public Admin(string role)
        {
            this._role = role;
        }

        /// <summary>
        /// Updates the admin with the event, logging it for future reference.
        /// </summary>
        /// <param name="event">The event to log.</param>
        public override void Update(Event @event)
        {
            this._logs.Add(@event);
        }

        /// <summary>
        /// Prints the logs of all events received by the admin.
        /// </summary>
        public void PrintLogs()
        {
            foreach (var log in this._logs)
            {
                System.Console.WriteLine($"[{log.CreatedAt}] - {log.Role.Name}.{log.Action}: {log.Message}");
            }
        }
    }
}
