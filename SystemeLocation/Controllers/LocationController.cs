using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SystemeLocation.Context;
using SystemeLocation.Entities;
using SystemeLocation.Models.Location;
using SystemeLocation.Models.Voiture;

namespace SystemeLocation.Controllers
{
    [Authorize(Roles = "Commis,Gérant,Administrateur")]
    public class LocationController : Controller
    {
        private readonly SystemeLocationContext _context;

        public LocationController(SystemeLocationContext context)
        {
            this._context = context;
        }

        public IActionResult Manage(Guid succursaleId)
        {
            var toList = _context.Locations.Where(location => location.Voiture.SuccursaleId == succursaleId);

            var vm = toList.Select(location => new LocationManageVM
            {
                LocationId = location.Id,
                Status = location.Status,
                OpeningTime = location.OpeningTime,
                PlannedClosingTime = location.PlannedClosingTime,
                ClosingTime = location.ClosingTime,
                VoitureName = location.Voiture.Name,
                Driver_Name = location.Conducteur.FirstName + " " + location.Conducteur.Name
            });

            ViewBag.succursaleId = succursaleId;

            var succursale = _context.Succursales.FirstOrDefault(succursale => succursale.Id == succursaleId);
            if (succursale.Status == StatutSuccursale.Activé)
                ViewBag.succursaleIsActive = true;
            else
                ViewBag.succursaleIsActive = false;

            return View(vm);
        }

        public IActionResult Create(Guid succursaleId)
        {
            var succursale = _context.Succursales.Find(succursaleId);

            if (succursale is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            // Prevents the access if you enter the url manually
            if (succursale.Status == StatutSuccursale.Désactivé)
                return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });

            var carNames = _context.Voitures.Where(v => v.SuccursaleId == succursaleId
                && v.Status == StatutVoiture.Activé && v.Available == Disponible.Vrai).Select(x => x.Name).Distinct().ToList();

            ViewBag.CarNames = new SelectList(carNames);
            ViewBag.succursaleId = succursaleId;

            DateTime now = DateTime.Now;
            DateTime openingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
            DateTime plannedClosingTime = openingTime.AddDays(3);

            return View(new LocationCreateVM()
            {
                OpeningTime = openingTime,
                PlannedClosingTime = plannedClosingTime,
            });
        }

        [HttpPost]
        public IActionResult Create(LocationCreateVM vm, Guid succursaleId)
        {
            var carNames = _context.Voitures.Where(v => v.SuccursaleId == succursaleId
               && v.Status == StatutVoiture.Activé && v.Available == Disponible.Vrai).Select(x => x.Name).Distinct().ToList();

            ViewBag.CarNames = new SelectList(carNames);
            ViewBag.succursaleId = succursaleId;

            if (vm.VoitureName == null)
            {
                ModelState.AddModelError(string.Empty, "You must choose a car!");
                return View(vm);
            }

            if (!ModelState.IsValid)
                return View(vm);

            var succursale = _context.Succursales.Find(succursaleId);

            if (succursale is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            if (!vm.IsExistingDriver)
            {
                var adresse = new Adresse()
                {
                    CivicNumber = vm.Address_CivicNumber,
                    Street = vm.Address_Street,
                    City = vm.Address_City,
                    PostalCode = vm.Address_PostalCode
                };

                var conducteur = new Conducteur()
                {
                    FirstName = vm.Driver_FirstName,
                    Name = vm.Driver_Name,
                    PhoneNumber = vm.Driver_Phone,
                    EmailAddress = vm.Driver_Email,
                    DriverLicense = vm.Driver_DriverLicense,
                    Adresse = adresse,
                };

                var location = new Location()
                {
                    Status = StatutLocation.Ouvert,
                    OpeningTime = vm.OpeningTime,
                    PlannedClosingTime = vm.PlannedClosingTime,
                    Voiture = _context.Voitures.FirstOrDefault(v => v.Name == vm.VoitureName),
                    Conducteur = conducteur,
                    Succursale = succursale,
                };

                if (!string.IsNullOrWhiteSpace(vm.Note))
                {
                    var note = new Note()
                    {
                        Location = location,
                        Description = vm.Note,
                    };
                    location.Notes.Add(note);
                }

                succursale.Locations.Add(location);

                _context.SaveChanges();
            }
            else
            {
                var conducteur = _context.Conducteurs.FirstOrDefault(c => c.DriverLicense == vm.Driver_DriverLicense);

                if (conducteur is null)
                {
                    ModelState.AddModelError(string.Empty, "The driver's license in not associated with an existing driver!");
                    return View(vm);
                }

                var location = new Location()
                {
                    Status = StatutLocation.Ouvert,
                    OpeningTime = vm.OpeningTime,
                    PlannedClosingTime = vm.PlannedClosingTime,
                    Voiture = _context.Voitures.FirstOrDefault(v => v.Name == vm.VoitureName),
                    Conducteur = conducteur,
                    Succursale = succursale,
                };

                if (!string.IsNullOrWhiteSpace(vm.Note))
                {
                    var note = new Note()
                    {
                        Location = location,
                        Description = vm.Note,
                    };
                    location.Notes.Add(note);
                }

                succursale.Locations.Add(location);

                _context.SaveChanges();
            }
            
            _context.Voitures.FirstOrDefault(v => v.Name == vm.VoitureName).Available = Disponible.Faux;
            _context.Voitures.FirstOrDefault(v => v.Name == vm.VoitureName).Status = StatutVoiture.Activé;

            _context.SaveChanges();

            return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });
        }

        public IActionResult Details(Guid succursaleId, Guid locationId)
        {
            var toShow = _context.Locations.Find(locationId);

            if (toShow is null)
                throw new ArgumentOutOfRangeException(nameof(locationId));

            var conducteur = _context.Conducteurs.FirstOrDefault(c => c.Id == toShow.ConducteurId);

            var adresse = _context.Adresses.FirstOrDefault(a => a.Id == conducteur.AdresseId);

            var voiture = _context.Voitures.FirstOrDefault(v => v.Id == toShow.VoitureId);

            var vm = new LocationDetailsVM()
            {
                Id = toShow.Id,
                Status = toShow.Status,
                OpeningTime = toShow.OpeningTime,
                PlannedClosingTime = toShow.PlannedClosingTime,
                ClosingTime = toShow.ClosingTime,
                VoitureName = voiture.Name,
                Driver_Name = conducteur.FirstName + " " + conducteur.Name,
                Driver_Phone = conducteur.PhoneNumber,
                Driver_Email = conducteur.EmailAddress,
                Driver_DriverLicense = conducteur.DriverLicense,
                Address_CivicNumber = adresse.CivicNumber,
                Address_Street = adresse.Street,
                Address_City = adresse.City,
                Address_PostalCode = adresse.PostalCode,
                Notes = _context.Notes.Where(n => n.LocationId == toShow.Id).ToList()
            };

            var succursale = _context.Succursales.FirstOrDefault(succursale => succursale.Id == succursaleId);
            if (succursale.Status == StatutSuccursale.Activé)
                ViewBag.succursaleIsActive = true;
            else
                ViewBag.succursaleIsActive = false;

            if (toShow.Status == StatutLocation.Ouvert)
                ViewBag.locationIsOpen = true;
            else
                ViewBag.locationIsOpen = false;

            ViewBag.succursaleId = succursaleId;
            ViewBag.locationId = locationId;

            return View(vm);
        }

        public IActionResult AddNote(Guid locationId, Guid succursaleId)
        {
            ViewBag.succursaleId = succursaleId;
            ViewBag.locationId = locationId;

            var location = _context.Locations.Find(locationId);

            if (location is null)
                throw new ArgumentOutOfRangeException(nameof(locationId));

            var succursale = _context.Succursales.Find(succursaleId);

            // Prevents the access if you enter the url manually
            if (succursale.Status == StatutSuccursale.Désactivé)
                return RedirectToAction(nameof(Details), new { succursaleId = succursaleId, locationId = locationId });

            if (location.Status == StatutLocation.Fermé)
                return RedirectToAction(nameof(Details), new { succursaleId = succursaleId, locationId = locationId });

            return View();
        }

        [HttpPost]
        public IActionResult AddNote(LocationAddNoteVM vm, Guid locationId, Guid succursaleId)
        {
            ViewBag.succursaleId = succursaleId;
            ViewBag.locationId = locationId;

            if (!ModelState.IsValid)
                return View(vm);

            var location = _context.Locations.Find(locationId);

            if (location is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            Note note = new Note()
            {
                Description = vm.Note,
                Location = location
            };

            location.Notes.Add(note);

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { succursaleId = succursaleId, locationId = locationId });
        }

        public IActionResult Close(Guid locationId, Guid succursaleId)
        {
            ViewBag.locationId = locationId;
            ViewBag.succursaleId = succursaleId;

            var location = _context.Locations.Find(locationId);

            if (location is null)
                throw new ArgumentOutOfRangeException(nameof(locationId));

            var succursale = _context.Succursales.Find(succursaleId);

            // Prevents the access if you enter the url manually
            if (succursale.Status == StatutSuccursale.Désactivé)
                return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });

            return View();
        }

        [HttpPost]
        public IActionResult Close(LocationCloseVM vm, Guid locationId, Guid succursaleId)
        {
            ViewBag.locationId = locationId;
            ViewBag.succursaleId = succursaleId;

            if (!ModelState.IsValid)
                return View(vm);

            var location = _context.Locations.Find(locationId);

            if (location is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            if (!string.IsNullOrWhiteSpace(vm.Note))
            {
                var note = new Note()
                {
                    Location = location,
                    Description = vm.Note,
                };
                location.Notes.Add(note);
            }

            location.ClosingTime = vm.ClosingTime;
            location.Status = StatutLocation.Fermé;

            var voiture = _context.Voitures.FirstOrDefault(v => v.Id == location.VoitureId);
            voiture.Available = Disponible.Vrai;

            _context.SaveChanges();

            return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });
        }
    }
}
