using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project.net.Data;
using project.net.Models;

namespace project.net.Controllers
{
    [Authorize(Roles ="Admin")] 
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<AppUser> 
    }
}
