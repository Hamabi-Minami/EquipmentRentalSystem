using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRentalSystem.Models
{
    public class RentalItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Equipment Equipment { get; set; }
        public int Quantity { get; set; }
        public double Cost { get; set; }

        [ForeignKey("RentalID")]
        public int RentalID { get; set; }
        public Rental Rental { get; set; }
    }
}
