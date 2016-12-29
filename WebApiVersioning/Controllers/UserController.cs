using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using WebApiVersioning.ModelBinder;
using WebApiVersioning.Models;

namespace WebApiVersioning.Controllers
{
    public class UserController : BaseController
    {
        public async Task<IHttpActionResult> Get()
        {
            return Ok(this.GetUsers());
        }

        public async Task<IHttpActionResult> Get(string Codice)
        {
            return Ok(this.GetUsers().Where(x => x.Codice == Codice).ToList());
        }

        public async Task<IHttpActionResult> Delete(string Codice)
        {
            var utenti = this.GetUsers().Where(x => x.Codice == Codice).ToList();
            if (!utenti.Any(x => x.Codice == Codice))
                return Conflict();

            return Ok();
        }

        public async Task<IHttpActionResult> Post([ModelBinder(typeof(UtenteModelBinder))] UtenteViewModel utente)
        {
            var utenti = this.GetUsers();
            if (utenti.Any(x => x.Codice == utente.Codice))
                return Conflict();

            return CreatedAtRoute("DefaultApi", new { Codice = utente.Codice }, utente);
        }
    }
    
    public static class UserControllerExtension
    {
        public static List<UtenteViewModel> GetUsers(this UserController ctr)
        {
            return GetUsersWithStartupData(10);
        }
        
        private static List<UtenteViewModel> GetUsersWithStartupData(int maxUser)
        {
            List<UtenteViewModel> utenti = new List<UtenteViewModel>();
            for (int i = 0; i < 10; i++)
            {
                utenti.Add(new UtenteViewModel
                {
                    Codice = $"US {i}",
                    DataNascita = DateTimeOffset.UtcNow.AddYears(-25).AddMonths(-i).AddDays(-i),
                    Descrizione = $"Utente n° {i}",
                    Email = "US{i}@mail.com",
                    Password = Guid.NewGuid().ToString("N"),
                    ScadenzaPassword = DateTimeOffset.UtcNow.AddHours(i),
                    Provincia = "VR"
                });
            }
            return utenti;
        }
    }    
}
