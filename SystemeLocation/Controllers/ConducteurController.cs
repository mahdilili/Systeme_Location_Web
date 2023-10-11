using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemeLocation.Context;
using SystemeLocation.Entities;
using SystemeLocation.Models.Conducteur;

namespace SystemeLocation.Controllers
{
    [Authorize(Roles = "Commis,Gérant,Administrateur")]
    public class ConducteurController : Controller
    {
        private readonly SystemeLocationContext _context;

        public ConducteurController(SystemeLocationContext context)
        {
            this._context = context;
        }

        public IActionResult Manage()
        {
            var vm = _context.Conducteurs.Select(conducteur => new ConducteurManageVM
            {
                Id = conducteur.Id,
                Name = $"{conducteur.FirstName} {conducteur.Name}",
                LastLocation = _context.Locations.Where(l => l.Conducteur.Name == conducteur.Name).OrderByDescending(l => l.OpeningTime).FirstOrDefault().OpeningTime,
                NbLocations = _context.Locations.Where(l => l.Conducteur.Name == conducteur.Name).Count()
            });

            return View(vm);
        }

        public IActionResult Details(Guid id)
        {
            var toShow = _context.Conducteurs.Find(id);

            if (toShow == null)
                throw new ArgumentOutOfRangeException(nameof(id));

            List<Location> locations = _context.Locations.Where(l => l.ConducteurId == id).ToList();

            var vm = new ConducteurDetailsVM()
            {
                Id = toShow.Id,
                FirstName = toShow.FirstName,
                LastName = toShow.Name,
                DriverLicense = toShow.DriverLicense,
                EmailAddress = toShow.EmailAddress,
                PhoneNumber = $"({toShow.PhoneNumber.Substring(0, 3)}) {toShow.PhoneNumber.Substring(3, 3)}-{toShow.PhoneNumber.Substring(6, 4)}",
                Locations = locations
            };

            return View(vm);
        }
    }
}
