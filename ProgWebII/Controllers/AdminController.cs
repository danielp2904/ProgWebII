using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AcademicEvents.Data;
using AcademicEvents.Models;

namespace AcademicEvents.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db) => _db = db;

        private bool IsAdmin =>
            HttpContext.Session.GetString("IsAdmin") == "1";

        private IActionResult Guard()
            => IsAdmin ? null! : RedirectToAction("AccessDenied", "Account");

        public IActionResult Index()
        {
            var gate = Guard(); if (gate != null) return gate;
            return View(_db.Events.OrderByDescending(e => e.Date).ToList());
        }

        public IActionResult Create()
        {
            var gate = Guard(); if (gate != null) return gate;
            return View(new Event());
        }

        [HttpPost]
        public IActionResult Create(Event e)
        {
            var gate = Guard(); if (gate != null) return gate;
            if (!ModelState.IsValid) return View(e);
            _db.Events.Add(e);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var gate = Guard(); if (gate != null) return gate;
            var e = _db.Events.Find(id);
            if (e == null) return NotFound();
            return View(e);
        }

        [HttpPost]
        public IActionResult Edit(Event e)
        {
            var gate = Guard(); if (gate != null) return gate;
            if (!ModelState.IsValid) return View(e);
            _db.Events.Update(e);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var gate = Guard(); if (gate != null) return gate;
            var e = _db.Events.Find(id);
            if (e == null) return NotFound();
            return View(e);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var gate = Guard(); if (gate != null) return gate;
            var e = _db.Events.Find(id);
            if (e != null)
            {
                _db.Events.Remove(e);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Lista de inscritos por evento
        public IActionResult Registrations(int id)
        {
            var gate = Guard(); if (gate != null) return gate;

            var ev = _db.Events.FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();

            var regs = _db.Registrations
                .Include(r => r.User)
                .Where(r => r.EventId == id)
                .OrderBy(r => r.RegisteredAt)
                .ToList();

            ViewBag.Event = ev;
            return View(regs);
        }
    }
}
