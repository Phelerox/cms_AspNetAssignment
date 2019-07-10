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
    [Authorize(Roles = "Administrator")]
    public class UserAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthorizationService _authorizationService;
        public UserAccountsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        // GET: UserAccounts
        public async Task<IActionResult> Index()
        {
            ViewBag.LinkText = "UserAccounts";
            var userAccounts = await _userManager.Users.ToListAsync();
            return View(userAccounts);
        }

        // GET: UserAccounts/Edit/admin@admin.com
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.LinkText = "UserAccounts";
            if (id == null || id == "")
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UserAccounts/Edit/admin@admin.com
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, bool isAdmin)
        {
            ViewBag.LinkText = "UserAccounts";
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var isCurrentlyAdmin = (await _userManager.GetRolesAsync(user)).Contains("Administrator");
            IdentityResult IR = null;
            if (isAdmin != isCurrentlyAdmin) {
                if (isAdmin) {
                    IR = await _userManager.AddToRoleAsync(user, "Administrator");
                }
                else {
                    IR = await _userManager.RemoveFromRoleAsync(user, "Administrator");
                }
            }
            if (IR != null && !IR.Succeeded) {
                ViewBag.ErrorMessage = IR.ToString();
                return View(user);
            }
            return View(user);
        }

        private bool UserAccountExists(string id)
        {
            return _userManager.FindByIdAsync(id).Result != null;
        }
    }
}
