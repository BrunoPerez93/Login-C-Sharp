using System.ComponentModel.DataAnnotations;

namespace LoginApp.ViewModels
{
    public class CarViewModel
    {
        public int IdCar { get; set; }
        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year is required")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Km is required")]
        public int Km { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        public bool Used { get; set; }
        public List<string> ExistingImages { get; set; } = new List<string>();
        public List<int> ImagesToDelete { get; set; } = new List<int>();

        public IFormFileCollection Images { get; set; }
    }
}
