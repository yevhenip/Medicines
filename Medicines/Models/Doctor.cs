namespace Medicines.Models
{
    public class Doctor
    {
        public Doctor()
        {
            Recipes = new HashSet<Recipe>();
        }

        public string Name { get; set; } = null!;
        public Guid Id { get; set; }
        public string License { get; set; } = null!;
        public Guid? HospitalId { get; set; }
        public int AmountOfRecipes { get; set; }

        public virtual Hospital? Hospital { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
