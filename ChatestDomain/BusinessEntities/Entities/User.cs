namespace ChatestDomain.BusinessEntities.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string NickName { get; set; }
    public List<Chat> Chats { get; set; }
}
