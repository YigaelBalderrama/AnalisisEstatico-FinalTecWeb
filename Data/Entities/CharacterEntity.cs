using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Data.Entities
{
    public class CharacterEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Occupation { get; set; }
        public bool? isProta { get; set; }
        public int? appearingSeason {get; set;}
        public virtual ICollection<PhraseEntity> Phrases { get; set; }
    }
}
