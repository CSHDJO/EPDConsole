using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Data
{
    public class Physician
    {
        [Key]
        public int PhysicianID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public int AddressID { get; set; }
        public Address? Address { get; set; }

        public string Phone { get; set; } = string.Empty;               

    }
}
