using Casino.Data;
using Casino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Casino.Controllers
{
    public class RouletteController : Controller
    {

        private readonly CasinoContext _context;

		public static int Period {  get; set; }

		public static string Message {  get; set; }

		public static int WinNumber {  get; set; }

		public static string WinColor {  get; set; }
		
		private readonly Dictionary<string, string> history;

		public RouletteController(CasinoContext context)
		{
			_context = context;
			history = new Dictionary<string, string>();
		}

		public IActionResult Index()
        {
			var uid = HttpContext.Session.GetInt32("LoggedId");
			if (uid == null)
			{
				return RedirectToAction("Login","Player");
			}

			var user = _context.Player.Find(uid);
			
			if (user.LastBetAt < DateTime.Now - TimeSpan.FromMinutes(5))
			{
				user.IsActive = false;
				ViewBag.NonActiveMsg = "You dont make bet more 5 minutes";

			}
			else
			{
				ViewBag.NonActiveMsg = "";
			}
		

			ViewBag.NickName = user.NickName;
			ViewBag.Period = Period;
			ViewBag.WinColor = WinColor;
			ViewBag.WinNumber = WinNumber;

			_context.SaveChanges();
			if (!String.IsNullOrEmpty(Message))
			{
				ViewBag.Msg = Message;
			}

			return View();


		}


		private void MakeBet(int number, string color, string oddEven, int money)
		{
			if(number == -1&& String.IsNullOrEmpty(color) && String.IsNullOrEmpty(oddEven))
			{
				Message = "You did not make bet";
				return;
			}



			var playerId = HttpContext.Session.GetInt32("LoggedId");



			if (playerId == null)
			{
				RedirectToAction("Player", "Login");
			}

			var player = _context.Player.Find(playerId);

			player.LastBetAt = DateTime.Now;
			player.IsActive = true;

			Message = "";
			var bet = new Bet
			{
				Color = color,
				number = number,
				Money = money,
				CreatedAt = DateTime.Now,
				EvenOdd = oddEven,
				PlayerId = (int)playerId
			};


			_context.Bet.Add(bet);
			_context.SaveChanges();

		//	return RedirectToAction()
		

		}





		public JsonResult ProcessBet(int number, string color, string oddEven, int money)
		{
			
			MakeBet(number, color, oddEven, money);

			var bets = _context.Bet.ToList();

			return new JsonResult(JsonSerializer.Serialize(bets));
				



		}


		public JsonResult GetHistory()
		{
			List<History> list = _context.History.OrderByDescending(x => x.createdAt).Take(10).ToList();
			return new JsonResult(JsonSerializer.Serialize(list));

		}




	}



}
