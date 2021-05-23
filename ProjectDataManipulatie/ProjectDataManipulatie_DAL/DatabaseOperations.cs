using ProjectDataManipulatie_DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProjectDataManipulatie_DAL
{
    public static class DatabaseOperations
    {
        public static List<dto.Persoon> GetAllPersons()
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                //return AtletiekinfoContext.tblPersoon.Where(p=>p.PersonenClubs.Count>0).ToList();
                return AtletiekinfoContext.tblPersoon.Include(x => x.PersonenClubs).Where(p => p.PersonenClubs.Count > 0).Select(
                    x => new dto.Persoon
                    {
                        Id = x.Id,
                        fullName = x.voornaam + " " + x.naam,
                        clubNaam = x.PersonenClubs.OrderByDescending(y => y.begin).FirstOrDefault().Club.naam,
                        borstNummer = x.PersonenClubs.OrderByDescending(y => y.begin).FirstOrDefault().borstNummer,
                        geslacht = x.geslacht
                    }).ToList();
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
        public static List<dto.Persoon> SearchPerson(bool? vlaanderen = true, bool? wallonie = true, string zoekData = "", int? provincieId = null, int? clubId = null)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                var Personen = AtletiekinfoContext.tblPersoon.AsQueryable();
                //.Include(x => x.PersonenClubs)

                //Only add athletes
                Personen = Personen.Include(x => x.PersonenClubs).Where(p=>p.PersonenClubs.Count>0);

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

                //return Personen.Select().ToList();
                return Personen.Select(
                    x => new dto.Persoon
                    {
                        Id = x.Id,
                        fullName = x.voornaam + " " + x.naam,
                        clubNaam = x.PersonenClubs.OrderByDescending(y => y.begin).FirstOrDefault().Club.naam,
                        borstNummer = x.PersonenClubs.OrderByDescending(y => y.begin).FirstOrDefault().borstNummer,
                        geslacht = x.geslacht
                    }).ToList();
            }
        }

        /// <summary>
        /// Get person by person id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Person object</returns>
        public static Persoon GetPersonById(int id)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblPersoon.Include(x => 
                x.PersonenClubs).Include(x=>
                x.PersonenClubs.Select(y=>y.Club))
                .Where(p => p.Id == id)
                .First();
            }
        }

        /// <summary>
        /// Check if email and password are correct
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>True or False (bool)</returns>
        public static bool CheckLogin(string email, string password)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                var login = AtletiekInfoContext.tblPersoon.Where(p => p.email == email && p.password == password).FirstOrDefault();
                if (login!=null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get person id by email adress
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Person id as Int</returns>
        public static int GetPersonIdByEmail(string email)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                var person = AtletiekInfoContext.tblPersoon.Where(p => p.email == email).First();
                return person.Id;
            }
        }

        /// <summary>
        /// Gets all unaccepted relation requests by person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>List of Relatie objects</returns>
        public static List<Relatie> GetUnacceptedRelationRequests(int personId)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                var requests = AtletiekinfoContext.tblRelaties.Where(x=> (x.Persoon1.Id == personId && x.Geaccepteerd == false) || x.Persoon2.Id == personId && x.Geaccepteerd == false);
                return requests.ToList();
            }
        }

        /// <summary>
        /// Gets relation information by relation id
        /// </summary>
        /// <param name="relationId"></param>
        /// <returns>Reatie object</returns>
        public static Relatie GetRelationById(int relationId)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblRelaties.Where(x=>x.Id == relationId).First();
            }
        }

        /// <summary>
        /// Gets sender of relation request by relation id
        /// </summary>
        /// <param name="relationId"></param>
        /// <returns>
        /// Persoon object
        /// </returns>
        public static Persoon GetRelationRequestSenderByRelationId(int relationId)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblRelaties.Where(x => x.Id == relationId).First().Persoon1;
            }
        }

        /// <summary>
        /// Get receiver of relation request by relation id
        /// </summary>
        /// <param name="relationId"></param>
        /// <returns>Persoon object</returns>
        public static Persoon GetRelationRequestReceiverByRelationId(int relationId)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                return AtletiekinfoContext.tblRelaties.Where(x => x.Id == relationId).First().Persoon2;
            }
        }

        /// <summary>
        /// Gets relation status between to persons
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="personId"></param>
        /// <returns>RelatieStatus enumm</returns>
        public static RelatieStatus GetRelationStatus(int currentUser, int personId)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                Relatie send = AtletiekInfoContext.tblRelaties.Where(x => (x.Persoon1.Id == currentUser && x.Persoon2.Id == personId) || (x.Persoon2.Id == currentUser && x.Persoon1.Id == personId)).FirstOrDefault();
                if (send != null)
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

        /// <summary>
        /// Get all accepted relations by person ID
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>List of persoon</returns>
        public static List<Persoon> GetFriends(int personId)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                List<Persoon> friends = new List<Persoon>();
                var relaties = AtletiekInfoContext.tblRelaties.Where(x => (x.Persoon1.Id == personId && x.Geaccepteerd == true) || (x.Persoon2.Id == personId && x.Geaccepteerd == true));
                foreach (var relatie in relaties)
                {
                    if (relatie.Persoon1.Id == personId)
                    {
                        friends.Add(relatie.Persoon2);
                    }
                    else
                    {
                        friends.Add(relatie.Persoon1);
                    }
                }
                return friends;
            }
        }

        //CREATE

        /// <summary>
        ///Inserts person into database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static bool CreatePerson(Persoon person)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                var check = AtletiekInfoContext.tblPersoon.Where(p => p.email == person.email).FirstOrDefault();
                if (check == null)
                {
                    int newId = AtletiekInfoContext.tblPersoon.OrderByDescending(p => p.Id).First().Id;
                    person.Id = newId + 1;
                    AtletiekInfoContext.tblPersoon.Add(person);
                    AtletiekInfoContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Creates unaccepted relation in database.
        /// </summary>
        /// <param name="personId1"></param>
        /// <param name="personId2"></param>
        public static void SendRelationRequest(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                Relatie r = new Relatie();
                r.Persoon1 = AtletiekInfoContext.tblPersoon.Where(p => p.Id == personId1).First();
                r.Persoon2 = AtletiekInfoContext.tblPersoon.Where(p => p.Id == personId2).First();
                AtletiekInfoContext.tblRelaties.Add(r);
                AtletiekInfoContext.SaveChanges();
            }
        }

        //UPDATE

        /// <summary>
        /// Update user person 
        /// </summary>
        /// <param name="idToUpdate"></param>
        /// <param name="email"></param>
        /// <param name="geboorteDatum"></param>
        public static void UpdateProfile(int idToUpdate, string email, DateTime geboorteDatum)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                Persoon user = AtletiekinfoContext.tblPersoon.Where(p => p.Id == idToUpdate).First();
                user.email = email;
                user.geboorteDatum = geboorteDatum;
                AtletiekinfoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Update person data in database
        /// </summary>
        /// <param name="person"></param>
        public static void UpdatePerson(Persoon person)
        {
            using (AtletiekInfoEntities AtletiekinfoContext = new AtletiekInfoEntities())
            {
                Persoon personToUpdate = AtletiekinfoContext.tblPersoon.Where(p => p.Id == person.Id).First();
                personToUpdate = person;
                AtletiekinfoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Set relation request to accepted in database
        /// </summary>
        /// <param name="personId1"></param>
        /// <param name="personId2"></param>
        public static void AcceptRelationRequest(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                var request = AtletiekInfoContext.tblRelaties.Where(x => x.Persoon1.Id == personId1 && x.Persoon2.Id == personId2).First();
                request.Geaccepteerd = true;
                AtletiekInfoContext.SaveChanges();
            }
        }

        //REMOVE

        /// <summary>
        /// Remove person and related data from database
        /// </summary>
        /// <param name="personId"></param>
        public static void DeletePersonById(int personId)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                //Get all relations related to selected person
                var personssRelations = AtletiekInfoContext.tblRelaties.Where(x => x.Persoon1.Id == personId || x.Persoon2.Id == personId).ToList();
                //Remove all relations in db
                personssRelations.ForEach(x=> AtletiekInfoContext.tblRelaties.Remove(x));
                //Remove person from db
                AtletiekInfoContext.tblPersoon.Remove(AtletiekInfoContext.tblPersoon.Where(p=>p.Id == personId).First());
                //Save changes
                AtletiekInfoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Remove relation from database
        /// </summary>
        /// <param name="personId1"></param>
        /// <param name="personId2"></param>
        public static void DeleteFriend(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                AtletiekInfoContext.tblRelaties.Remove(AtletiekInfoContext.tblRelaties.Where(x => (x.Persoon1.Id == personId1 && x.Persoon2.Id == personId2) || (x.Persoon2.Id == personId1 && x.Persoon1.Id == personId2)).First());
                AtletiekInfoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Remove send relation request from database
        /// </summary>
        /// <param name="personId1"></param>
        /// <param name="personId2"></param>
        public static void CancelRelationRequest(int personId1, int personId2)
        {
            using (AtletiekInfoEntities AtletiekInfoContext = new AtletiekInfoEntities())
            {
                AtletiekInfoContext.tblRelaties.Remove(AtletiekInfoContext.tblRelaties.Where(x => x.Persoon1.Id == personId1 && x.Persoon2.Id == personId2).First());
                AtletiekInfoContext.SaveChanges();
            }
        }
    }
}
