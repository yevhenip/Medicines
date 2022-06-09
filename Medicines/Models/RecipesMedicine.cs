namespace Medicines.Models
{
    public class RecipesMedicine
    {
        public Guid RecipeId { get; set; }
        public Guid MedicineId { get; set; }
        public int Count { get; set; }

        public virtual Medicine Medicine { get; set; } = null!;
        public virtual Recipe Recipe { get; set; } = null!;
    }
}
