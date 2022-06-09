namespace Medicines.Models
{
    public class Recipe
    {
        public Recipe()
        {
            RecipesMedicines = new HashSet<RecipesMedicine>();
            Patients = new HashSet<Patient>();
        }

        public Guid Id { get; set; }
        public DateTime DateOfGiving { get; set; }
        public Guid DoctorId { get; set; }
        public int ValidityPeriod { get; set; }

        public virtual Doctor Doctor { get; set; } = null!;
        public virtual ICollection<RecipesMedicine> RecipesMedicines { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
