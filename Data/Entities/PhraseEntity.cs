using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpsonApp.Data.Entities
{
    public class PhraseEntity
    {
        public int ID { get; set; }
       
        public string Content { get; set; }
       
        public int Season { get; set; }
       
        public string Popularity { get; set; }
        public virtual CharacterEntity Character { get; set; }
    }
}
