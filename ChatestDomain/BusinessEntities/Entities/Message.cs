namespace ChatestDomain.BusinessEntities.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Chat Chat { get; set; }
    public User User { get; set; }
    public DateTime DateTime { get; set; }
}
