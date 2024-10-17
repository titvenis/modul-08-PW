using System;
using System.Collections.Generic;

public interface IMediator
{
    void SendMessage(string message, IUser user, string channel);
    void AddUser(IUser user, string channel);
    void RemoveUser(IUser user, string channel);
}

public class ChatMediator : IMediator
{
    private Dictionary<string, List<IUser>> channels = new Dictionary<string, List<IUser>>();

    public void AddUser(IUser user, string channel)
    {
        if (!channels.ContainsKey(channel))
        {
            channels[channel] = new List<IUser>();
        }

        channels[channel].Add(user);
        NotifyUsers($"{user.Name} присоединился к каналу {channel}", channel);
    }

    public void RemoveUser(IUser user, string channel)
    {
        if (channels.ContainsKey(channel))
        {
            channels[channel].Remove(user);
            NotifyUsers($"{user.Name} покинул канал {channel}", channel);
        }
    }

    public void SendMessage(string message, IUser user, string channel)
    {
        if (channels.ContainsKey(channel) && channels[channel].Contains(user))
        {
            foreach (var u in channels[channel])
            {
                if (u != user)
                {
                    u.ReceiveMessage(message, user.Name, channel);
                }
            }
        }
        else
        {
            Console.WriteLine($"Ошибка: {user.Name} не может отправить сообщение в канал {channel}. Пользователь не состоит в канале.");
        }
    }

    private void NotifyUsers(string message, string channel)
    {
        foreach (var user in channels[channel])
        {
            user.ReceiveNotification(message);
        }
    }
}

public interface IUser
{
    string Name { get; }
    void SendMessage(string message, string channel);
    void ReceiveMessage(string message, string fromUser, string channel);
    void ReceiveNotification(string notification);
}

public class User : IUser
{
    private IMediator mediator;
    public string Name { get; private set; }

    public User(IMediator mediator, string name)
    {
        this.mediator = mediator;
        this.Name = name;
    }

    public void SendMessage(string message, string channel)
    {
        Console.WriteLine($"{Name} отправляет сообщение: {message} в канал {channel}");
        mediator.SendMessage(message, this, channel);
    }

    public void ReceiveMessage(string message, string fromUser, string channel)
    {
        Console.WriteLine($"{Name} получил сообщение от {fromUser} в канале {channel}: {message}");
    }

    public void ReceiveNotification(string notification)
    {
        Console.WriteLine($"{Name} получил уведомление: {notification}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        IMediator chatMediator = new ChatMediator();

        IUser user1 = new User(chatMediator, "Alice");
        IUser user2 = new User(chatMediator, "Bob");
        IUser user3 = new User(chatMediator, "Charlie");

        chatMediator.AddUser(user1, "general");
        chatMediator.AddUser(user2, "general");

        user1.SendMessage("Привет", "general");

        chatMediator.AddUser(user3, "tech");

        user2.SendMessage("Как дела?", "general");
        user3.SendMessage("Привет, ребята!", "tech");

        chatMediator.RemoveUser(user2, "general");
        user1.SendMessage("Ты еще здесь?", "general");
    }
}
