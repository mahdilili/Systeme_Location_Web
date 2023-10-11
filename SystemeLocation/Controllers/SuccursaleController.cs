using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemeLocation.Context;
using SystemeLocation.Entities;
using SystemeLocation.Models.Succursale;

namespace SystemeLocation.Controllers
{
    [Authorize(Roles = "Commis,Gérant,Administrateur")]
    public class SuccursaleController : Controller
    {
        private readonly SystemeLocationContext _context;

        public SuccursaleController(SystemeLocationContext context)
        {
            this._context = context;
        }

        public IActionResult Manage()
        {
            var vm = _context.Succursales.Select(succursale => new SuccursaleManageVM
            {
                Id = succursale.Id,
                Name = succursale.Name,
                Status = succursale.Status,
                CarCount = succursale.Voitures.Where(v => v.Status == StatutVoiture.Activé || v.Status == StatutVoiture.Désactivé).Count(),
                ActiveCars = succursale.Voitures.Where(v => v.Status == StatutVoiture.Activé).Count(),
                AvailableCars = succursale.Voitures.Where(v => v.Available == Disponible.Vrai).Count()
            });

            return View(vm);
        }

        [Authorize(Roles = "Administrateur")]
        public IActionResult Create()
        {
            int nbSuccursales = _context.Succursales.Count();
            nbSuccursales++;

            return View(new SuccursaleCreateVM()
            {
                Name = "Branch" + nbSuccursales.ToString()
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrateur")]
        public IActionResult Create(SuccursaleCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            if (vm.Address_PostalCode is not null && vm.Address_CivicNumber is not null && vm.Address_City is not null 
                && vm.Address_Street is not null)
            {
                var adresse = new Adresse()
                {
                    Street = vm.Address_Street.Trim(),
                    CivicNumber = vm.Address_CivicNumber,
                    City = vm.Address_City.Trim(),
                    PostalCode = vm.Address_PostalCode.Trim()
                };

                var succursale = new Succursale()
                {
                    Name = vm.Name.Trim(),
                    Adresse = adresse
                };

                _context.Succursales.Add(succursale);
            }
            else
            {
                var succursale = new Succursale()
                {
                    Name = vm.Name.Trim()
                };

                _context.Succursales.Add(succursale);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "Administrateur")]
        public IActionResult Edit(Guid id)
        {
            var toEdit = _context.Succursales.Find(id);

            if (toEdit is null)
                throw new ArgumentOutOfRangeException(nameof(id));

            if (toEdit.Status == StatutSuccursale.Activé)
            {
                toEdit.Status = StatutSuccursale.Désactivé;
                _context.SaveChanges();
            }
            else if (toEdit.Status == StatutSuccursale.Désactivé)
            {
                toEdit.Status = StatutSuccursale.Activé;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "Administrateur")]
        public IActionResult Delete(Guid id)
        {
            var toDeleteSuc = _context.Succursales.Find(id);

            if (toDeleteSuc is null)
                throw new ArgumentOutOfRangeException(nameof(id));


            if (toDeleteSuc.Status == StatutSuccursale.Désactivé)
            {
                var toDeleteLoc = _context.Locations.Where(location => location.SuccursaleId == toDeleteSuc.Id);
                foreach (Location l in toDeleteLoc)
                {
                    var toDeleteLocationNotes = _context.Notes.Where(n => n.LocationId == l.Id);
                    foreach (var note in toDeleteLocationNotes)
                        _context.Notes.Remove(note);

                    _context.Locations.Remove(l);
                }

                var toDeleteVoiture = _context.Voitures.Where(voiture => voiture.SuccursaleId == toDeleteSuc.Id);
                foreach (Voiture v in toDeleteVoiture)
                {
                    var toDeleteVoitureNotes = _context.Notes.Where(n => n.VoitureId == v.Id);
                    foreach (var note in toDeleteVoitureNotes)
                    {
                        _context.Notes.Remove(note);
                    }
                    _context.Voitures.Remove(v);
                }

                if (toDeleteSuc.AdresseId is not null)
                {
                    var toDeleteAdr = _context.Adresses.Find(toDeleteSuc.AdresseId);
                    _context.Adresses.Remove(toDeleteAdr);
                }

                _context.Succursales.Remove(toDeleteSuc);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}