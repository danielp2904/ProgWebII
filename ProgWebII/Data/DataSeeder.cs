using AcademicEvents.Models;
using AcademicEvents.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace AcademicEvents.Data
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            db.Database.EnsureCreated();

            if (!db.Users.Any())
            {
                PasswordHasher.CreatePassword("admin@ifc.local", "Admin@123", out var hash, out var salt);
                db.Users.Add(new User
                {
                    Name = "Administrador",
                    Email = "admin@ifc.local",
                    PasswordHash = hash,
                    Salt = salt,
                    IsAdmin = true
                });
                PasswordHasher.CreatePassword("user@ifc.local", "User@123", out var hash2, out var salt2);
                db.Users.Add(new User
                {
                    Name = "Usuário",
                    Email = "user@ifc.local",
                    PasswordHash = hash2,
                    Salt = salt2,
                    IsAdmin = false
                });
                db.SaveChanges();
            }

            if (!db.Events.Any())
            {
                db.Events.AddRange(
                    new Event { Name = "Semana Acadêmica", Date = DateTime.Today.AddDays(7), Location = "Auditório A", Description = "Palestras e workshops." },
                    new Event { Name = "Mostra de Pesquisa", Date = DateTime.Today.AddDays(20), Location = "Bloco B", Description = "Apresentação de projetos." }
                );
                db.SaveChanges();
            }
        }
    }
}
