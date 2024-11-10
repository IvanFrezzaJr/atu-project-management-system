

public class Event
{
    public Role Role { get; set; }
    public string Action { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; } = DateTime.Now;

    public Event(Role role, string action, string message)
    {
        Role = role;
        Action = action;
        Message = message;
    }
}

public interface ISubscriber
{
    void Update(Event @event);
}

public interface IPublisher
{
    void AddSubscriber(ISubscriber subscriber);
    void NotifyObservers(Event @event);
}


public abstract class Subscriber : ISubscriber
{
    public abstract void Update(Event @event);
}



public abstract class Publisher : IPublisher
{
    private List<ISubscriber> _subscribers = new List<ISubscriber>();
    
    public void AddSubscriber(ISubscriber subscriber)
    {
        
        if (!this._subscribers.Contains(subscriber)) {
            this._subscribers.Add(subscriber);
        }
    }

    public void NotifyObservers(Event @event) {
        foreach (var subscriber in this._subscribers)
        {
            subscriber.Update(@event);
        }
    }

}


public class Role : Publisher
{
    public string Name { get; set; }

    public Role(string name)
    {
        Name = name;
    }
}


public class Student : Role
{
    public Student(string name) : base(name) { }

    public void Submit(string message)
    {
        System.Console.WriteLine($"{Name} has submiting: {message}");

        Event @event = new Event(this, "Print", message);
        this.NotifyObservers(@event);
    }
}

public class Admin : Subscriber
{
    private string _role = string.Empty;
    private List<Event> _logs = new List<Event>();

    public Admin(string Admin)
    {
        this._role = Admin;
    }

    public override void Update(Event @event)
    {
       this._logs.Add(@event);
    }

    public void PrintLogs()
    {
        foreach (var log in this._logs)
        {
            System.Console.WriteLine($"[{log.CreatedAt}] - {log.Role.Name}.{log.Action}: {log.Message}");
        }
    }
}


internal class Program
{
    static void Main(string[] args)
    {
        Student ivan = new Student("Ivan");
        Student bruna = new Student("Bruna");
        Admin admin = new Admin("Super admin");
        ivan.AddSubscriber(admin);
        bruna.AddSubscriber(admin);

        ivan.Submit("Hello");
        bruna.Submit("World");

        admin.PrintLogs();
    }
}
