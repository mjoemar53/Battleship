var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureServices((context, services) =>
        {
            services.AddSingleton<IGameService, GameService>();
        });

var host = hostBuilder.Build();
var gameService = host.Services.GetService<IGameService>() ?? throw new NullReferenceException();

var player1 = new GamePlayer("Marc", false);
var player2 = new GamePlayer("Computer1", true);

gameService.Start();
gameService.Setup(player1);
gameService.Setup(player2);
gameService.FinishSetup();
gameService.BattlePhase();