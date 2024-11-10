

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


internal class Program
{
    static void Main(string[] args)
    {
       
    }
}
