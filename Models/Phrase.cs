using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Models
{
    public class Phrase
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Required")]
        [StringLength(100, ErrorMessage = "The phrase it`s to large the max lenght is 100 characters")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Required")]
        [Range(1,32, ErrorMessage = "The are only 32 seasons")]
        public int? Season { get; set; }
        [StringLength(7,ErrorMessage = "Error {0} the max leng is{1} min lenght is {2}", MinimumLength = 3)]
        public string Popularity { get; set; }
        public int CharacterID { get; set; }
    }
}
