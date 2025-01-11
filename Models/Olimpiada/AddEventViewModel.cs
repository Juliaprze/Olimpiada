using System.ComponentModel.DataAnnotations;

namespace Olimpiada.Models.Olimpiada
{
    public class AddEventViewModel
    {
        public int AthleteId { get; set; }

        [Required(ErrorMessage = "Nazwa dyscypliny jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa dyscypliny może zawierać maksymalnie 100 znaków.")]
        public string SportName { get; set; }

        [Required(ErrorMessage = "Nazwa wydarzenia jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa wydarzenia może zawierać maksymalnie 100 znaków.")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Nazwa olimpiady jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa olimpiady może zawierać maksymalnie 100 znaków.")]
        public string Olympics { get; set; }

        [Required(ErrorMessage = "Wiek sportowca jest wymagany.")]
        [Range(1, 120, ErrorMessage = "Wiek sportowca musi być w zakresie od 1 do 120 lat.")]
        public int AthleteAge { get; set; }
    }

}