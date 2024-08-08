using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRentalSystem.Models
{
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public required DateTime RentalDate { get; set; }
        public required DateTime ReturnDate { get; set; }
        public required Customer Customer { get; set; }
        //public required Equipment Equipment { get; set; }
        public required Double Cost { get; set; }

        public List<RentalItem> RentalItems { get; set; } = new List<RentalItem>();

        [NotMapped]
        public string RentalItemsDisplay => string.Join(", ", RentalItems.Select(ri => $"{ri.Equipment.Name}*{ri.Quantity}"));
    }
}
