namespace Medicines.Models
{
    public class Medicine
    {
        public Medicine()
        {
            RecipesMedicines = new HashSet<RecipesMedicine>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public int Price { get; set; }

        public virtual ICollection<RecipesMedicine> RecipesMedicines { get; set; }
    }
}
