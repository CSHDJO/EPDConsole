using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chipsoft.Data
{
    public class Address
    {
        [Key]
        [Column(Order = 1)]
        public int AddressID { get; set; }
        [Column(Order = 2)]
        public string Country { get; set; } = string.Empty;
        [Column(Order = 3)]
        public string City { get; set; } = string.Empty;
        [Column(Order = 4)]
        public string PostalCode { get; set; } = string.Empty;
        [Column(Order = 5)]
        public string Street { get; set; } = string.Empty;
        [Column(Order = 6)]
        public string HouseNumber { get; set; } = string.Empty;
    }
}
