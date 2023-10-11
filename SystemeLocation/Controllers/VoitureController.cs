using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemeLocation.Context;
using SystemeLocation.Entities;
using SystemeLocation.Models.Voiture;

namespace SystemeLocation.Controllers
{
    [Authorize(Roles = "Commis,Gérant,Administrateur")]
    public class VoitureController : Controller
    {
        private readonly SystemeLocationContext _context;

        public VoitureController(SystemeLocationContext context)
        {
            this._context = context;
        }

        public IActionResult Manage(Guid succursaleId)
        {
            var toList = _context.Voitures.Where(voiture => voiture.SuccursaleId == succursaleId && voiture.Status != StatutVoiture.Archivé);

            var vm = toList.Select(voiture => new VoitureManageVM
            {
                Id = voiture.Id,
                Name = voiture.Name,
                RegistrationNumber = voiture.RegistrationNumber,
                Brand = voiture.Brand,
                Model = voiture.Model,
                Year = voiture.Year,
                Color = voiture.Color,
                Status = voiture.Status
            });

            ViewBag.succursaleId = succursaleId;

            var succursale = _context.Succursales.FirstOrDefault(succursale => succursale.Id == succursaleId);
            if (succursale.Status == StatutSuccursale.Activé)
                ViewBag.succursaleIsActive = true;
            else
                ViewBag.succursaleIsActive = false;

            return View(vm);
        }

        [Authorize(Roles = "Gérant,Administrateur")]
        public IActionResult Create(Guid succursaleId)
        {
            var succursale = _context.Succursales.Find(succursaleId);

            if (succursale is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            // Prevents the access if you enter the url manually
            if (succursale.Status == StatutSuccursale.Désactivé)
                return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });

            int nbVoitures = _context.Voitures.Where(v => v.SuccursaleId == succursaleId).Count();
            nbVoitures++;
            string voitureName = "";
            if (nbVoitures < 9)
                voitureName = "Car0" + nbVoitures.ToString();
            else
                voitureName = "Car" + nbVoitures.ToString();

            if (succursale is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            ViewBag.succursaleId = succursaleId;

            return View(new VoitureCreateVM()
            {
                Name = voitureName
            });
        }

        [HttpPost]
        [Authorize(Roles = "Gérant,Administrateur")]
        public IActionResult Create(VoitureCreateVM vm, Guid succursaleId)
        {
            ViewBag.succursaleId = succursaleId;

            if (!ModelState.IsValid)
                return View(vm);

            bool carAlreadyExists = _context.Voitures.Any(u => u.SerialNumber == vm.SerialNumber);

            if (carAlreadyExists)
            {
                ModelState.AddModelError(string.Empty, "A car with this serial number already exists.");
                return View(vm);
            }

            var succursale = _context.Succursales.Find(succursaleId);

            if (succursale is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            var voiture = new Voiture()
            {
                SuccursaleId = succursaleId,
                Name = vm.Name.Trim(),
                Status = vm.Status,
                Available = vm.Available,
                State = vm.State,
                SerialNumber = vm.SerialNumber.Trim(),
                RegistrationNumber = vm.RegistrationNumber.Trim(),
                Brand = vm.Brand.Trim(),
                Model = vm.Model.Trim(),
                Year = vm.Year,
                Color = vm.Color.Trim(),
                Mileage = vm.Mileage,
                EstimatePrice = vm.EstimatePrice
            };

            if (!string.IsNullOrWhiteSpace(vm.Note))
            {
                var note = new Note()
                {
                    Description = vm.Note,
                    Voiture = voiture
                };
                voiture.Notes.Add(note);
            }
            
            succursale.Voitures.Add(voiture);

            _context.SaveChanges();

            return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });
        }

        public IActionResult Details(Guid voitureId, Guid succursaleId)
        {
            var toShow = _context.Voitures.Find(voitureId);

            if (toShow is null)
                throw new ArgumentOutOfRangeException(nameof(voitureId));

            var vm = new VoitureDetailsVM()
            {
                Id = toShow.Id,
                Name = toShow.Name.Trim(),
                Status = toShow.Status,
                Available = toShow.Available,
                State = toShow.State,
                SerialNumber = toShow.SerialNumber.Trim(),
                RegistrationNumber = toShow.RegistrationNumber.Trim(),
                Brand = toShow.Brand.Trim(),
                Model = toShow.Model.Trim(),
                Year = toShow.Year,
                Color = toShow.Color.Trim(),
                Mileage = toShow.Mileage,
                EstimatePrice = toShow.EstimatePrice,
                SuccursaleName = _context.Succursales.Find(toShow.SuccursaleId).Name,
                Notes = _context.Notes.Where(n => n.VoitureId == toShow.Id).ToList(),
                AssociatedLocations = _context.Locations.Where(l => l.Voiture == toShow).ToList()
            };

            var succursale = _context.Succursales.FirstOrDefault(succursale => succursale.Id == succursaleId);
            if (succursale.Status == StatutSuccursale.Activé)
                ViewBag.succursaleIsActive = true;
            else
                ViewBag.succursaleIsActive = false;

            if (toShow.Status == StatutVoiture.Activé)
                ViewBag.voitureIsActive = true;
            else
                ViewBag.voitureIsActive = false;

            ViewBag.succursaleId = succursaleId;
            ViewBag.voitureId = voitureId;

            return View(vm);
        }

        public IActionResult Edit(Guid voitureId, Guid succursaleId)
        {
            var toEdit = _context.Voitures.Find(voitureId);

            if (toEdit is null)
                throw new ArgumentOutOfRangeException(nameof(voitureId));

            if (toEdit.Status == StatutVoiture.Activé)
            {
                toEdit.Status = StatutVoiture.Désactivé;
                toEdit.Available = Disponible.Faux;
                _context.SaveChanges();
            }
            else if (toEdit.Status == StatutVoiture.Désactivé && (User.IsInRole("Gérant") || User.IsInRole("Administrateur")))
            {
                toEdit.Status = StatutVoiture.Activé;
                toEdit.Available = Disponible.Vrai;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });
        }

        [Authorize(Roles = "Gérant,Administrateur")]
        public IActionResult Archive(Guid voitureId, Guid succursaleId)
        {
            var toArchive = _context.Voitures.Find(voitureId);

            if (toArchive is null)
                throw new ArgumentOutOfRangeException(nameof(voitureId));

            if (toArchive.Status == StatutVoiture.Désactivé)
            {
                toArchive.Status = StatutVoiture.Archivé;
                toArchive.Available = Disponible.Faux;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Manage), new { succursaleId = succursaleId });
        }

        public IActionResult AddNote(Guid voitureId, Guid succursaleId)
        {
            ViewBag.succursaleId = succursaleId;
            ViewBag.voitureId = voitureId;

            var voiture = _context.Voitures.Find(voitureId);

            if (voiture is null)
                throw new ArgumentOutOfRangeException(nameof(voitureId));

            var succursale = _context.Succursales.Find(succursaleId);

            // Prevents the access if you enter the url manually
            if (succursale.Status == StatutSuccursale.Désactivé || voiture.Status != StatutVoiture.Activé)
                return RedirectToAction(nameof(Details), new { succursaleId = succursaleId, voitureId = voitureId });

            return View();
        }

        [HttpPost]
        public IActionResult AddNote(VoitureAddNoteVM vm, Guid voitureId, Guid succursaleId)
        {
            ViewBag.succursaleId = succursaleId;
            ViewBag.voitureId = voitureId;

            if (!ModelState.IsValid)
                return View(vm);

            var voiture = _context.Voitures.Find(voitureId);

            if (voiture is null)
                throw new ArgumentOutOfRangeException(nameof(succursaleId));

            Note note = new Note()
            {
                Description = vm.Note,
                Voiture = voiture
            };

            voiture.Notes.Add(note);

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { succursaleId = succursaleId, voitureId = voitureId });
        }
    }
}
