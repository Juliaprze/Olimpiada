using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Olimpiada.Models.Olimpiada;

namespace Olimpiada.Controllers
{
    [Route("athletes")]
    [ApiController]
    public class AthletesController : Controller
    {
        private readonly OlympicsContext _context;

        public AthletesController(OlympicsContext context)
        {
            _context = context;
        }

        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var totalAthletes = _context.People.Count();

            var athletes = _context.People
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(person => new AthleteViewModel
                {
                    FullName = person.FullName ?? "Nieznane imię i nazwisko",
                    Weight = person.Weight ?? null,
                    Height = person.Height ?? null,
                    Gender = person.Gender ?? "Nieznana płeć",
                    GoldMedals = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id && ce.Medal.MedalName == "Gold"),
                    SilverMedals = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id && ce.Medal.MedalName == "Silver"),
                    BronzeMedals = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id && ce.Medal.MedalName == "Bronze"),
                    CompetitionsCount = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id),
                    CompetitionsLink = person.Id.ToString()
                })
                .OrderBy(person => person.FullName)
                .ToList();

            var viewModel = new AthleteListViewModel
            {
                TotalAthletes = totalAthletes,
                CurrentPage = page,
                PageSize = pageSize,
                Athletes = athletes
            };

            return View(viewModel);
        }


        [HttpGet("{id}/competitions")]
        public IActionResult GetCompetitions(int id)
        {
            var competitions = _context.CompetitorEvents
                .Where(ce => ce.Competitor.PersonId == id)
                .Include(ce => ce.Event.Sport)
                .Include(ce => ce.Medal)
                .AsEnumerable() // Przejście na IEnumerable, by obsłużyć warunki po stronie aplikacji
                .Select(ce => new CompetitionDetailViewModel
                {
                    SportName = ce.Event?.Sport?.SportName ?? "Nieznany sport",
                    EventName = ce.Event?.EventName ?? "Nieznane wydarzenie",
                    Olympics = "Nieznana olimpiada", 
                    Season = "Nieznany sezon",       
                    AthleteAge = null,              
                    Medal = ce.Medal?.MedalName == null || ce.Medal.MedalName == "NA" ? "0" : ce.Medal.MedalName // Obsługa "NA"
                })
                .ToList();

            return View("GetCompetitions", new CompetitionsDetailViewModel
            {
                AthleteId = id,
                Competitions = competitions
            });
        }



        [HttpGet("{id}/add-event")]
        public IActionResult AddEvent(int id)
        {
            var athlete = _context.People.FirstOrDefault(p => p.Id == id);
            if (athlete == null)
            {
                return NotFound("Athlete not found.");
            }

            ViewBag.AthleteName = athlete.FullName;
            ViewBag.Events = _context.Events.Select(e => new { e.Id, e.EventName }).ToList();

            return View(new AddEventViewModel { AthleteId = id });
        }

        [HttpPost("{id}/add-event")]
        public IActionResult AddEvent(AddEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Events = _context.Events.Select(e => new { e.Id, e.EventName }).ToList();
                return View(model);
            }

            var newEvent = new CompetitorEvent
            {
                CompetitorId = model.AthleteId,
                EventId = model.EventId,
                MedalId = model.MedalId
            };

            _context.CompetitorEvents.Add(newEvent);
            _context.SaveChanges();

            return RedirectToAction("GetCompetitions", new { id = model.AthleteId });
        }

        [HttpGet("{id}/competition-details")]
        public IActionResult GetCompetitionDetails(int id)
        {
            var competitions = _context.CompetitorEvents
                .Where(ce => ce.Competitor.PersonId == id)
                .Include(ce => ce.Event.Sport)
                .Include(ce => ce.Competitor.Person)
                .AsEnumerable()
                .Select(ce => new CompetitionDetailViewModel
                {
                    SportName = ce.Event?.Sport?.SportName ?? "Nieznany sport",
                    EventName = ce.Event?.EventName ?? "Nieznane wydarzenie",
                    Olympics = "Nieznana olimpiada",
                    Season = "Nieznany sezon",
                    AthleteAge = null, // Dynamiczne obliczenia wieku można tu dodać
                    Medal = !string.IsNullOrEmpty(ce.Medal?.MedalName) ? ce.Medal.MedalName : "0" 
                })
                .ToList();

            return View("GetCompetitionDetails", new CompetitionsDetailViewModel
            {
                AthleteId = id,
                Competitions = competitions
            });
        }

    }

    public class AthleteViewModel
    {
        public string FullName { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public string Gender { get; set; }
        public int GoldMedals { get; set; }
        public int SilverMedals { get; set; }
        public int BronzeMedals { get; set; }
        public int CompetitionsCount { get; set; }
        public string CompetitionsLink { get; set; }
    }

    public class AthleteListViewModel
    {
        public int TotalAthletes { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<AthleteViewModel> Athletes { get; set; }
    }

    public class CompetitionDetailViewModel
    {
        public string SportName { get; set; }
        public string EventName { get; set; }
        public string Olympics { get; set; }
        public string Season { get; set; }
        public int? AthleteAge { get; set; }
        public string Medal { get; set; }
    }

    public class CompetitionsDetailViewModel
    {
        public int AthleteId { get; set; }
        public List<CompetitionDetailViewModel> Competitions { get; set; }
    }

    public class AddEventViewModel
    {
        public int AthleteId { get; set; }
        public int EventId { get; set; }
        public int? MedalId { get; set; }
    }
}
