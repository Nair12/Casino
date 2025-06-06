using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Casino.Data;
using Casino.Models;

namespace Casino.Controllers
{
    public class PlayerController : Controller
    {
        private readonly CasinoContext _context;

        public PlayerController(CasinoContext context)
        {
            _context = context;
        }

        // GET: Player
        public async Task<IActionResult> Index()
        {
            var uid = HttpContext.Session.GetInt32("LoggedId");
            if (uid == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Player.FindAsync(uid);
            var userHistory = await _context.Archive.Where(item => item.PlayerId == uid).ToListAsync();

            foreach(var item in userHistory)
            {
                item.BetInfo = String.Join(" ",item.BetInfo.Split('-'));
            }

            ViewBag.Archive = userHistory;

               
            return View(user);
        }

        [HttpGet]

        public IActionResult Register()
        {

            return View();

        }

        [HttpPost]

        public IActionResult Register(Player player,IFormFile avatar)
        {
            if(player == null)
            {

            }

            player.Balance = 100;
            player.totalBet = 0;
            player.totalWins = 0;
            player.CreatedAt = DateTime.Now;

            using (MemoryStream ms = new MemoryStream())
            {
                avatar.CopyTo(ms);

                var base64 = Convert.ToBase64String(ms.ToArray());
                player.Avatar = base64;

            }
            _context.Player.Add(player);
            _context.SaveChanges();
           
            return RedirectToAction("Login");

        }


        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedId");
            HttpContext.Session.Remove("LoggedName");
            return RedirectToAction("Login");

        }





        [HttpGet]

        public IActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public IActionResult Login(string nickName, string password)
        {

            var player = _context.Player.FirstOrDefault(x => x.NickName == nickName && x.Password == password);

            if (player != null)
            {
                HttpContext.Session.SetInt32("LoggedId", player.Id);
                HttpContext.Session.SetString("LoggedName", player.NickName);
				player.LastBetAt = DateTime.Now;
				player.IsActive = true;
                  
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Login");
            }






        }



        [HttpGet("balance/{id}")]
        public JsonResult GetBalanceById(int id)
        {
            if(id == 0)
            {
                return Json(null);
            }
            var player = _context.Player.Find(id);

            if(player != null)
            {
                return Json(new { balance = player.Balance });
            }
            else
            {
                return Json(new { balance = 0 });
            }
        }
    }
}
