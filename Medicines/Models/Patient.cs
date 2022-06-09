namespace Medicines.Models
{
    public class Patient
    {
        public Patient()
        {
            Recipes = new HashSet<Recipe>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
