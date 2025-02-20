using ChatestDomain.BusinessEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatestInfrastructure.Storage;

// Контекст базы данных для работы с SQLite
public class SQLiteDbContext : DbContext
{
    // Определение таблицы сообщений
    public DbSet<Message> Messages { get; set; }

    // Определение таблицы чатов
    public DbSet<Chat> Chats { get; set; }

    // Определение таблицы пользователей
    public DbSet<User> Users { get; set; }

    // Конструктор, создающий базу данных при необходимости
    public SQLiteDbContext()
    {
        Database.EnsureCreated(); // Гарантирует, что база данных создана
    }

    // Настройка конфигурации подключения к базе данных
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = chatest.db"); // Используем SQLite и указываем файл БД
    }
}
