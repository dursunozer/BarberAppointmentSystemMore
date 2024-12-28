﻿using System.ComponentModel.DataAnnotations;

namespace BarberAppointmentSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Services> Services { get; set; } 

    }
}
