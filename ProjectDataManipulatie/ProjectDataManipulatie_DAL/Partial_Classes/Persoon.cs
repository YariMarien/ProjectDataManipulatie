using System;
using System.Linq;
using System.Net.Mail;

namespace ProjectDataManipulatie_DAL
{
    public partial class Persoon : BasisKlasse
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
                if (this.PersonenClubs != null)
                {
                    return this.PersonenClubs.OrderByDescending(x => x.begin).First().Club;
                }
                else
                {
                    return null;
                }
            }
        }
        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(email))
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        return "Email is verplicht in te vullen.";
                    }
                    else
                    {
                        try
                        {
                            MailAddress m = new MailAddress(email);
                        }
                        catch (Exception)
                        {
                            return "Ongeldig email adres.";
                        }
                    }
                }

                if (columnName == nameof(geboorteDatum) && geboorteDatum.Year > (DateTime.Now.Year - 6))
                {
                    return "Je moet minstens 6 jaar oud zijn.";
                }
                return "";
            }
        }
    }
}
