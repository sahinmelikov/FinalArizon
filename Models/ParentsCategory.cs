namespace FinalArizon.Models
{
    public class ParentsCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public List<Model> Models { get; set; } = new List<Model>();
        public List<Feature> Features { get; set; }
    }
}
