using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CareManagement.Data;
using CareManagement.Models.AUTH;
using CareManagement.Models.OM;
using CareManagement.Models.SCHDL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CareManagement.Controllers.AUTH
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public const String ADMIN_ROLE = HomeController.ADMIN_ROLE;
        public const String EMPLOYEE_ROLE = HomeController.EMPLOYEE_ROLE;
        private IdentityRole admin;
        private IdentityRole employee;

        private readonly CareManagementContext _context;
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private RoleManager<IdentityRole> roleManager;

        public AdminController(CareManagementContext context, UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash, RoleManager<IdentityRole> roleMgr)
        {
            _context = context;
            userManager = usrMgr;
            passwordHasher = passwordHash;
            roleManager = roleMgr;

            Task createRoleTask = getRole();
            Task.WaitAll(createRoleTask);
        }

        public async Task getRole()
        {
            admin = await roleManager.FindByNameAsync(ADMIN_ROLE);
            employee = await roleManager.FindByNameAsync(EMPLOYEE_ROLE);
            return;
        }

        public async Task addUserToRole(String userId, String roleName)
        {
            IdentityRole roleAdded = roleName == ADMIN_ROLE ? admin : employee;
            IdentityRole roleRemoved = roleName == ADMIN_ROLE ? employee : admin;
            AppUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.AddToRoleAsync(user, roleAdded.Name);
                await userManager.RemoveFromRoleAsync(user, roleRemoved.Name);
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(userManager.Users);
        }

        // GET: Admin/Create
        public async Task<IActionResult> Create()
        {
            var employees = await _context.Employee.ToListAsync();
            ViewData["EmployeeList"] = new SelectList(employees.Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName + " - " + e.Title }), "EmployeeId", "FullName");

            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Password,Role,EmployeeId")] User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    EmployeeId = user.EmployeeId
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                await addUserToRole(appUser.Id, user.Role);

                if (result.Succeeded) {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employee.ToListAsync();
            ViewData["EmployeeList"] = new SelectList(employees.Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName + " - " + e.Title }), "EmployeeId", "FullName");

            AppUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String id, String username, String email, String role, Guid employeeId, String password, String? newPassword)
        {
            var employees = await _context.Employee.ToListAsync();
            ViewData["EmployeeList"] = new SelectList(employees.Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName + " - " + e.Title }), "EmployeeId", "FullName");

            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                bool allow = true;

                if (string.IsNullOrEmpty(password) || passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Success)
                {
                    ModelState.AddModelError("", "Password does not match");
                    allow = false;
                }

                if (!string.IsNullOrEmpty(username))
                    user.UserName = username;
                else
                {
                    ModelState.AddModelError("", "Username cannot be empty");
                    allow = false;
                }

                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                    allow = false;
                }

                if (!string.IsNullOrEmpty(role))
                {
                    user.Role = role;
                    await addUserToRole(user.Id, role);
                }
                else
                {
                    ModelState.AddModelError("", "Role cannot be empty");
                    allow = false;
                }

                if (employeeId != null)
                {
                    user.EmployeeId = employeeId;
                }
                else
                {
                    ModelState.AddModelError("", "Employee ID cannot be empty");
                    allow = false;
                }

                if (!string.IsNullOrEmpty(newPassword))
                    user.PasswordHash = passwordHasher.HashPassword(user, newPassword);

                if (allow)
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return RedirectToAction("Index");
        }
        
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}