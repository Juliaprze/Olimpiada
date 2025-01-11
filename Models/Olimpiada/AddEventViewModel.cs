using System.ComponentModel.DataAnnotations;

namespace Olimpiada.Models.Olimpiada
{
    public class AddEventViewModel
    {
        [Required(ErrorMessage = "Nazwa dyscypliny jest wymagana.")]
        [StringLength(50, ErrorMessage = "Nazwa dyscypliny może mieć maksymalnie 50 znaków.")]
        public string SportName { get; set; }

        [Required(ErrorMessage = "Nazwa wydarzenia jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa wydarzenia może mieć maksymalnie 100 znaków.")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Nazwa olimpiady jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa olimpiady może mieć maksymalnie 100 znaków.")]
        public string Olympics { get; set; }

        [Range(10, 100, ErrorMessage = "Wiek sportowca musi mieścić się w zakresie od 10 do 100 lat.")]
        public int AthleteAge { get; set; }

        public int AthleteId { get; set; }
    }
}