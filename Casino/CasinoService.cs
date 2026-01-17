
using Casino.Controllers;
using Casino.Data;
using Casino.Models;

namespace Casino
{
	public class CasinoService : BackgroundService
	{
		static int Period = 60;
		static int WinNumber;
		static string WinColor;



		//private readonly CasinoContext _context;

		private readonly IServiceScopeFactory _scopeFactory;

		public CasinoService(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		protected async override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using (var scope = _scopeFactory.CreateScope())

			using (var context = scope.ServiceProvider.GetRequiredService<CasinoContext>())


			while (!stoppingToken.IsCancellationRequested)
			{ 
				Period--;
				if(Period <= 1)
				{
					WinNumber = new Random().Next(0, 37);
					WinColor = new Random().Next(0, 100) % 2 == 0 ? "black" : "red";
					if (WinNumber == 0) WinColor = "green";

					await ProcessBet(context);

					Period = 60;
				}
				RouletteController.Period = Period;
				RouletteController.WinNumber = WinNumber;
				RouletteController.WinColor = WinColor;

				await Task.Delay(1000,stoppingToken);
			}
		}

		private async Task ProcessBet(CasinoContext context)
		{
			var bets = context.Bet.ToList();


			foreach (var bet in bets) {

				int factor = 0;
				if (bet.number == WinNumber)
				{
					factor += 35;
				}
				if(bet.Color == WinColor)
				{
					factor++;
				}
				if(bet.EvenOdd == "even" && WinNumber % 2 == 0)
				{
					factor++;
				}
				if (bet.EvenOdd == "odd" && WinNumber % 2 != 0)
				{
					factor++;
				}
             if (factor > 0) factor++;
			 else factor--;

				var player = context.Player.Find(bet.PlayerId);
				if (player != null) {

					player.Balance += factor*bet.Money;
					player.totalBet++;
					if (factor > 0) player.totalWins++;


					
				}
				PlayerArchive info = new PlayerArchive() {
					PlayerId = bet.PlayerId,
					CreatedTime = DateTime.Now,
					BetInfo = WinNumber + "-" + WinColor + "-" + bet.EvenOdd,
				    Money = bet.Money,
					Result = factor * bet.Money,

				};
				await context.Archive.AddAsync(info);



			}
			context.Bet.RemoveRange(bets);

			var historyItem = new History() { Color = WinColor, Number = WinNumber, createdAt = DateTime.Now };

			await context.History.AddAsync(historyItem);
			await context.SaveChangesAsync();
			
		}
	}
}
