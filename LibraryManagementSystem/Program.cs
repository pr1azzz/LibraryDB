using System;
using System.Windows;
using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("=== Библиотечная система ===");
            Console.WriteLine("Запуск приложения...");
            
            try
            {
                // Проверка подключения к БД
                using (var context = new LibraryContext())
                {
                    context.Database.EnsureCreated();
                    Console.WriteLine("✅ Подключение к базе данных успешно!");
                    Console.WriteLine($"   База данных: {context.Database.GetDbConnection().Database}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка подключения к БД: {ex.Message}");
                Console.WriteLine("   Проверьте:");
                Console.WriteLine("   - Запущен ли PostgreSQL");
                Console.WriteLine("   - Правильный ли пароль в LibraryContext.cs");
                Console.WriteLine("   - Правильный ли порт (5433)");
            }

            Console.WriteLine("🪟 Открытие главного окна...");
            Console.WriteLine("================================\n");
            
            // Запуск WPF приложения
            App app = new App();
            app.Run();
            
            Console.WriteLine("👋 Приложение завершено");
        }
    }
}