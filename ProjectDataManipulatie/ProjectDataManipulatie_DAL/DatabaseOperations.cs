using ProjectDataManipulatie_DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataManipulatie_DAL
{
    public static class DatabaseOperations
    {
        public static List<Persoon> GetAllPersons()
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblPersoon.ToList();
            }
        }
        public static List<Provincie> GetAllProvinces()
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblProvincies.ToList();
            }
        }
        public static List<Provincie> GetProvincesByRegion(bool? vlaanderen, bool? wallonië)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                if (wallonië == true && vlaanderen == true)
                {
                    return AtletiekinfoContext.tblProvincies.Where(x =>
                    x.gewest == true
                    || x.gewest == false
                    ).ToList();
                }
                else if (vlaanderen == true)
                {
                    return AtletiekinfoContext.tblProvincies.Where(x =>
                    x.gewest == false
                    ).ToList();
                }
                else if (wallonië == true)
                {
                    return AtletiekinfoContext.tblProvincies.Where(x =>
                    x.gewest == true
                    ).ToList();
                }
                else
                    return AtletiekinfoContext.tblProvincies.ToList();
            }
        }
        public static List<Club> GetAllClubs()
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblClub.ToList();
            }
        }
        public static List<Club> GetClubsByProvince(int? provinceId)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                    return AtletiekinfoContext.tblClub.Where(x=> x.provincieId == provinceId).ToList();
            }
        }
        public static List<Club> GetClubsByRegion(bool? vlaanderen, bool? wallonië)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                if(vlaanderen == true && wallonië == true)
                {
                    return AtletiekinfoContext.tblClub.Where(x =>
                        x.Provincie.gewest == true
                        || x.Provincie.gewest == false
                    ).ToList();
                }
                else if (wallonië == true)
                {
                    return AtletiekinfoContext.tblClub.Where(x =>
                        x.Provincie.gewest == true
                    ).ToList();
                }
                else
                {
                    return AtletiekinfoContext.tblClub.Where(x =>
                         x.Provincie.gewest == false
                     ).ToList();
                }
            }
        }
        public static List<Persoon> SearchPerson(bool? vlaanderen, bool? wallonie, string zoekData, int? provincieId, int? clubId)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                var Personen = AtletiekinfoContext.tblPersoon.AsQueryable();

                ////Add search data to query
                if (!string.IsNullOrEmpty(zoekData))
                    //Personen = Personen.Where(p => );
                    Personen = Personen.Where(p =>
                    (p.voornaam + " " + p.naam).Contains(zoekData)
                    || (p.naam + " " + p.voornaam).Contains(zoekData)
                    || p.PersonenClubs.Where(y => y.begin.Year == DateTime.Now.Year && y.borstNummer.Contains(zoekData)).Any()
                    );

                ////Add region to query
                if (vlaanderen == true && wallonie == true)
                {
                    Personen = Personen.Where(p =>
                    p.PersonenClubs.OrderByDescending(x => x.begin).FirstOrDefault().Club.Provincie.gewest == true
                    || p.PersonenClubs.OrderByDescending(x => x.begin).FirstOrDefault().Club.Provincie.gewest ==false
                    );
                }
                else if (wallonie == true)
                {
                    Personen = Personen.Where(p =>
                    p.PersonenClubs.OrderByDescending(x => x.begin).FirstOrDefault().Club.Provincie.gewest == true
                    );
                }
                else if (vlaanderen == true)
                {
                    Personen = Personen.Where(p =>
                    p.PersonenClubs.OrderByDescending(x => x.begin).FirstOrDefault().Club.Provincie.gewest == false
                    );
                }

                //Add province to query
                if (provincieId != null)
                {
                    Personen = Personen.Where(p =>
                    p.PersonenClubs.OrderByDescending(x => x.begin).FirstOrDefault().Club.provincieId == provincieId
                    );
                }

                //Add club to query
                if (clubId != null)
                {
                    Personen = Personen.Where(p =>
                    p.PersonenClubs.OrderByDescending(x => x.begin).FirstOrDefault().clubId == clubId
                    );
                }

                return Personen.ToList();
            }
        }

        public static Persoon GetPersonById(int id)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblPersoon.Where(p=> p.Id == id).First();
            }
        }

        public static List<Relatie> GetUnacceptedRelationRequests(int personId)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                var requests = AtletiekinfoContext.tblRelaties.Where(x=> (x.Persoon1.Id == personId && x.Geaccepteerd == false) || x.Persoon2.Id == personId && x.Geaccepteerd == false);
                return requests.ToList();
            }
        }

        public static RelatieStatus GetRelationStatus(int currentUser, int personId)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                Relatie send = AtletiekInfoContext.tblRelaties.Where(x => (x.Persoon1.Id == currentUser && x.Persoon2.Id == personId) || (x.Persoon2.Id == currentUser && x.Persoon1.Id == personId)).FirstOrDefault();
                if (send!=null)
                {
                    if (send.Geaccepteerd)
                    {
                        return RelatieStatus.Vrienden;
                    }
                    else if (send.Persoon1.Id == currentUser)
                    {
                        return RelatieStatus.VerzoekVerzonden;
                    }
                    else
                    {
                        return RelatieStatus.VerzoekOntvangen;
                    }
                }
                else
                {
                    return RelatieStatus.NietVerbonden;
                }
            }
        }

        public static void SendRelationShipRequest(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                Relatie r = new Relatie();
                r.Persoon1 = AtletiekInfoContext.tblPersoon.Where(p=>p.Id==personId1).First();
                r.Persoon2 = AtletiekInfoContext.tblPersoon.Where(p => p.Id == personId2).First();
                AtletiekInfoContext.tblRelaties.Add(r);
                AtletiekInfoContext.SaveChanges();
            }
        }

        public static void CancelRelationShipRequest(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                AtletiekInfoContext.tblRelaties.Remove(AtletiekInfoContext.tblRelaties.Where(x=>x.Persoon1.Id == personId1 && x.Persoon2.Id == personId2).First());
                AtletiekInfoContext.SaveChanges();
            }
        }

        public static void AcceptRelationShipRequest(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                var request = AtletiekInfoContext.tblRelaties.Where(x => x.Persoon1.Id == personId1 && x.Persoon2.Id == personId2).First();
                request.Geaccepteerd = true;
                AtletiekInfoContext.SaveChanges();
            }
        }

        public static void DeleteFriend(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                AtletiekInfoContext.tblRelaties.Remove(AtletiekInfoContext.tblRelaties.Where(x => (x.Persoon1.Id == personId1 && x.Persoon2.Id == personId2) || (x.Persoon2.Id == personId1 && x.Persoon1.Id == personId2)).First());
                AtletiekInfoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Get persons current club
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Current club as Club</returns>
        public static Club GetCurrentClub(this Persoon person)
        {
            var id = person.Id;
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                var club = AtletiekinfoContext.tblPersoon.Where(p => p.Id == id).First().PersonenClubs.OrderByDescending(x => x.begin).First().Club;
                return club;
            }
        }

        /// <summary>
        /// Get persons current chest number
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Current chest number as string</returns>
        public static string GetCurrentNumber(this Persoon person)
        {
            var id = person.Id;
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                var nmbr = AtletiekinfoContext.tblPersoon.Where(p => p.Id == id).First().PersonenClubs.OrderByDescending(x => x.begin).First().borstNummer;
                return nmbr;
            }
        }


        //UPDATE

        public static void UpdatePerson(int idToUpdate, string email, DateTime geboorteDatum)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                Persoon user = AtletiekinfoContext.tblPersoon.Where(p => p.Id == idToUpdate).First();
                user.email = email;
                user.geboorteDatum = geboorteDatum;
                AtletiekinfoContext.SaveChanges();
            }
        }
    }
}
