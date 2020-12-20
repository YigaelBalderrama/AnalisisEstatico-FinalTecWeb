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
        [Required(ErrorMessage="Required")]
        [StringLength(50, ErrorMessage = "The name it's to large the max lenght is 50 characters")]
        public string  Name { get; set; }
        [Required]
        [Range(1,99,ErrorMessage ="The range of age it is (1-99) years")]
        public int? Age { get; set; }
        [StringLength(35,ErrorMessage ="The limit of letters it's 35")]
        public string Occupation { get; set; }
        public bool? isProta { get; set; }
        [Range(1, 32, ErrorMessage = "There are only 32 seasons until now.")]
        public int? appearingSeason { get; set; }
        public IEnumerable<Phrase> Phrases { get; set; }
    }
}
