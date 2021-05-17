using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataManipulatie_DAL
{
    public partial class Persoon
    {
        public string FullName
        {
            get
            {
                return voornaam + " " + naam;
            }
        }
        public Club CurrentClub
        {
            get
            {
                return this.GetCurrentClub();
            }
        }
        public string CurrentNumber
        {
            get
            {
                return this.GetCurrentNumber();
            }
        }
    }
}
