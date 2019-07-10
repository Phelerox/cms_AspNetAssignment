using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cms.Data;
using cms.Models;
using cms.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace cms.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;
        public PersonsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        // GET: Persons
        public async Task<IActionResult> Index()
        {
            ViewBag.LinkText = "Persons";
            var applicationDbContext = _context.Persons.Where(p => p.Status > PersonStatus.Hidden || (p.CreatorId == _userManager.GetUserId(User) || User.IsInRole("Administrator"))).Include(p => p.City);
            var persons = await applicationDbContext.ToListAsync();
            // var tasks = persons.Select(async person => new { person, filter = await _authorizationService.AuthorizeAsync(
            //          User, person,
            //          PersonOperations.Read) });
            // var results = await Task.WhenAll(tasks);
            // persons = (List<Person>) results.Where(p => p.filter.Succeeded).Select(p => p.person);
            return View(persons);
        }

        // GET: Persons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.LinkText = "Persons";
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Persons
                .Include(p => p.City)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null || (person.Status <= PersonStatus.Hidden && !(person.CreatorId == _userManager.GetUserId(User) || User.IsInRole("Administrator"))))
            {
                return NotFound();
            }
            ViewData["Creator"] = (await _userManager.FindByIdAsync(person.CreatorId)).UserName;
            return View(person);
        }

        // GET: Persons/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.LinkText = "PersonsEdit";
            var person = new Person() {
                CreatorId = _userManager.GetUserId(User)
            };
             if (!(await _authorizationService.AuthorizeAsync(
                     User, person,
                     PersonOperations.Create)).Succeeded)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities.Include(c => c.Country), "CityId", null); //null makes it use ToString() https://stackoverflow.com/questions/25873494/how-make-selectlist-use-tostring-method-for-displaying-elements-and-set-datava
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,Name,Description,CityId,Phone,Portrait,Email,Status")] Person person)
        {
            ViewBag.LinkText = "PersonsEdit";
            person.CreatorId = _userManager.GetUserId(User);
             if(!(await _authorizationService.AuthorizeAsync(
                     User, person,
                     PersonOperations.Create)).Succeeded)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities.Include(c => c.Country), "CityId", null);
            return View(person);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.LinkText = "PersonsEdit";
            if (id == null)
            {
                var applicationDbContext = _context.Persons.Where((p) => (p.CreatorId == _userManager.GetUserId(User) || User.IsInRole("Administrator"))).Include(p => p.City);
                var persons = await applicationDbContext.ToListAsync();
                return View("EditIndex", persons);
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null || !(await _authorizationService.AuthorizeAsync(
                     User, person,
                     PersonOperations.Update)).Succeeded)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities.Include(c => c.Country), "CityId", null, person.CityId);
            return View(person);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,Name,Description,CityId,Phone,Portrait,Email,Status")] Person person)
        {
            ViewBag.LinkText = "PersonsEdit";
            if (id != person.PersonId || !(await _authorizationService.AuthorizeAsync(
                     User, person,
                     PersonOperations.Update)).Succeeded)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities.Include(c => c.Country), "CityId", null, person.CityId);
            return View(person);
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.LinkText = "PersonsEdit";
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.City)
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null || !(await _authorizationService.AuthorizeAsync(
                     User, person,
                     PersonOperations.Delete)).Succeeded)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.LinkText = "PersonsEdit";
            var person = await _context.Persons.FindAsync(id);
            if(!(await _authorizationService.AuthorizeAsync(
                     User, person,
                     PersonOperations.Delete)).Succeeded)
            {
                return NotFound();
            }
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.PersonId == id);
        }
    }
}
