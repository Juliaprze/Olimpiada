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

        [HttpGet]
        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { Error = "Page and pageSize must be greater than 0." });
            }

            var totalAthletes = _context.People.Count();

            var athletes = _context.People
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(person => new AthleteViewModel
                {
                    FullName = person.FullName,
                    Weight = person.Weight ?? 0, // Obsługa wartości null
                    Height = person.Height ?? 0,
                    Gender = person.Gender,
                    GoldMedals = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id && ce.Medal.MedalName == "Gold"),
                    SilverMedals = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id && ce.Medal.MedalName == "Silver"),
                    BronzeMedals = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id && ce.Medal.MedalName == "Bronze"),
                    CompetitionsCount = _context.CompetitorEvents.Count(ce => ce.Competitor.PersonId == person.Id),
                    CompetitionsLink = Url.Action("GetCompetitions", new { id = person.Id })
                })
                .OrderBy(person => person.FullName)
                .AsNoTracking()
                .ToList();

            var viewModel = new AthleteListViewModel
            {
                TotalAthletes = totalAthletes,
                CurrentPage = page,
                PageSize = pageSize,
                Athletes = athletes
            };

            return View(viewModel); // Wygeneruje widok listy
        }

        [HttpGet("{id}/competitions")]
        public IActionResult GetCompetitions(int id)
        {
            try
            {
                var competitions = _context.CompetitorEvents
                    .Where(ce => ce.Competitor.PersonId == id)
                    .AsEnumerable() // Konwertujemy do IEnumerable, aby użyć operatorów LINQ w pamięci
                    .Select(ce => new CompetitionViewModel
                    {
                        EventId = ce.EventId ?? 0, // Domyślna wartość, jeśli EventId jest null
                        EventName = ce.Event?.EventName ?? "Unknown" // Bezpieczny dostęp do EventName
                    })
                    .ToList();

                return View(new CompetitionsViewModel
                {
                    AthleteId = id,
                    Competitions = competitions
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd: {ex.Message}");
            }
        }

    }

    public class AthleteViewModel
    {
        public string FullName { get; set; }
        public int Weight { get; set; } // Zmiana na `int` z domyślną wartością
        public int Height { get; set; }
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

    public class CompetitionViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
    }

    public class CompetitionsViewModel
    {
        public int AthleteId { get; set; }
        public List<CompetitionViewModel> Competitions { get; set; }
    }
}
