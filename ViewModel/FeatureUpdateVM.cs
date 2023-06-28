namespace FinalArizon.ViewModel
{
    public class FeatureUpdateVM
    {
        public int Id { get; set; }
        public string AboutProduct { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public double Vol { get; set; } ////hecmi
        public int Tension { get; set; }  ////// Gerginlik

        public string Size { get; set; }
        public double Weight { get; set; } ///Cekisi

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}
