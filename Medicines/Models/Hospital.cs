namespace Medicines.Models
{
    public class Hospital
    {
        public Hospital()
        {
            Doctors = new HashSet<Doctor>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
