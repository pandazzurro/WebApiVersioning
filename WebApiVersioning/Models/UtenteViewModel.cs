using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiVersioning.Models
{
    public class UtenteViewModel
    {
        [Required]
        [Key]
        public string Codice { get; set; }
        
        public string Descrizione { get; set; }

        [StringLength(25)]
        public string Provincia { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset ScadenzaPassword { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset DataNascita { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}