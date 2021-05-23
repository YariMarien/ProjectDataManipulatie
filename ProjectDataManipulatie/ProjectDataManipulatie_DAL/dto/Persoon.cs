using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataManipulatie_DAL.dto
{
    public class Persoon
    {
        public int Id { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string clubNaam { get; set; }
        public string geslacht { get; set; }
        public string borstNummer { get; set; }
        public string password { get; set; }
        public DateTime geboorteDatum { get; set; }
    }
}
