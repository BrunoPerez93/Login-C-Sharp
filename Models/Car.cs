namespace LoginApp.Models
{
    public class Car
    {
        public int IdCar { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Km { get; set; }
        public bool Used { get; set; }
        public string Color { get; set; }
        public List<byte[]> Images { get; set; }
    }
}
