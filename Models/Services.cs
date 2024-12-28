using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarberAppointmentSystem.Models
{
    public class Services
    {
        [Key]
        public int ServiceId { get; set; } // Servis ID

        [Required]
        public string Name { get; set; } // Servis adı

        public string Description { get; set; } // Servis açıklaması

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } // Servis ücreti

        public ICollection<Employee> Employees { get; set; } // Servise atanmış çalışanlar
    }
}
