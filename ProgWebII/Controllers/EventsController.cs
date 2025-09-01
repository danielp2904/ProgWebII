using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AcademicEvents.Data;
using AcademicEvents.Models;

namespace AcademicEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EventsController(ApplicationDbContext db) => _db = db;

        private int? CurrentUserId => HttpContext.Session.GetInt32("UserId");

        private bool RequireLogin()
        {
            if (CurrentUserId == null)
            {
                return true;
            }
            return false;
        }

        // GET: /Events
        public IActionResult Index()
        {
            if (RequireLogin()) return RedirectToAction("Login", "Account");
            var eventsList = _db.Events.OrderBy(e => e.Date).ToList();
            return View(eventsList);
        }

        // POST: /Events/Register/5
        [HttpPost]
        public IActionResult Register(int id)
        {
            if (RequireLogin()) return RedirectToAction("Login", "Account");
            var uid = CurrentUserId!.Value;

            var exists = _db.Registrations.Any(r => r.UserId == uid && r.EventId == id);
            if (!exists)
            {
                _db.Registrations.Add(new Registration { UserId = uid, EventId = id });
                _db.SaveChanges();
                TempData["msg"] = "Inscrição realizada!";
            }
            else
            {
                TempData["msg"] = "Você já está inscrito neste evento.";
            }
            return RedirectToAction("Index");
        }

        // GET: /Events/MyRegistrations
        public IActionResult MyRegistrations()
        {
            if (RequireLogin()) return RedirectToAction("Login", "Account");

            var uid = CurrentUserId!.Value;
            var regs = _db.Registrations
                .Include(r => r.Event)
                .Where(r => r.UserId == uid)
                .OrderBy(r => r.Event!.Date)
                .ToList();

            return View(regs);
        }
    }
}
