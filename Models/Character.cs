using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Models
{
    public class Character
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The name it`s to large the max lenght is 50 characters")]
        public string  Name { get; set; }
        [Required]
        [Range(1,99,ErrorMessage ="The range of age it is (1-99) years")]
        public int Age { get; set; }
        [StringLength(35,ErrorMessage ="The limit its 35")]
        public string Descripcion { get; set; }
        public bool isProta { get; set; }
        public IEnumerable<Phrase> Phrases { get; set; }
    }
}
