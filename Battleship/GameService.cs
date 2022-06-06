namespace Battleship
{
    public class GameService : IGameService
    {
        private bool isSetupDone;
        private bool isGameOver;
        private readonly int boardSize = 10;
        private readonly int maxShips = 3;
        private readonly List<GamePlayer> playerList;
        public GameService()
        {
            playerList = new List<GamePlayer>();
        }
        public void Start()
        {
            Console.WriteLine("A new battleship game has begun...");
            isSetupDone = false;
            isGameOver = false;
        }

        public void Setup(GamePlayer player)
        {
            Console.WriteLine();
            Console.WriteLine($"Setting up {player.Name} board");
            player.Board = CreateBoard();
            if (!player.IsAI)
            {
                while(player.ShipsPlaced + 1 <= maxShips)
                {
                    PrintBoard(player.Board);
                    var shipType = player.ShipSelectionMenu();
                    var rowNumber = player.ShipPlacementMenu("row", boardSize) - 1;
                    var colNumber = player.ShipPlacementMenu("column", boardSize) - 1;
                    var isHorizontal = player.ShipOrientationMenu();
                    var orientationText = isHorizontal ? "Horizontally to the right" : "Vertically down";

                    var isValid = HasValidShipPlacement(rowNumber, colNumber, shipType, player.Board, isHorizontal);
                    if (isValid)
                    {
                        Placeship(rowNumber, colNumber, shipType, player.Board, isHorizontal);
                        PrintBoard(player.Board);
                        Console.WriteLine($"{shipType} placed starting at [{rowNumber + 1}, {colNumber + 1}] {orientationText}.");
                        player.AddShip();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Placement");
                        break;
                    }
                }
            } 
            else
            {
                Placeship(1, 1, Types.Battleship, player.Board, true);
                player.AddShip();
                Placeship(3, 3, Types.Carrier, player.Board, false);
                player.AddShip();
                Placeship(8, 1, Types.Submarine, player.Board, true);
                player.AddShip();
                PrintBoard(player.Board);
            }
            playerList.Add(player);
        }

        public void FinishSetup()
        {
            if (playerList.Count != 2)
            {
                Console.WriteLine("Setup 2 players first");
                return;
            }
            isSetupDone = true;
            isGameOver = false;
        }

        public void BattlePhase()
        {
            if(!isSetupDone)
            {
                Console.WriteLine("Finish Setup first");
                return;
            }
            var player1 = playerList[0];
            var player2 = playerList[1];

            Console.WriteLine();
            Console.WriteLine("Battlephase...");
            while (!isGameOver) {
                var targetCell = player1.Attack(boardSize);
                var attackResult = AttackPlayer(player2, targetCell.Item1, targetCell.Item2);
                if (attackResult)
                {
                    Console.WriteLine($"{player1.Name} attack landed a hit");
                }
                else
                {
                    Console.WriteLine($"{player1.Name} attack missed");
                }

                if(isGameOver)
                {
                    Console.WriteLine($"{player1.Name} won");
                    break;
                }

                targetCell = player2.Attack(boardSize);
                attackResult = AttackPlayer(player1, targetCell.Item1, targetCell.Item2);
                if (attackResult)
                {
                    Console.WriteLine($"{player2.Name} attack landed a hit");
                }
                else
                {
                    Console.WriteLine($"{player2.Name} attack missed");
                }
                if (isGameOver)
                {
                    Console.WriteLine($"{player2.Name} won");
                    break;
                }
            }
        }

        private bool HasValidShipPlacement(int row, int col, Types shipType, Types[,] board, bool isHorizontal)
        {
            var limit = isHorizontal ? col + (int)shipType : row + (int)shipType;

            bool hasConflict = false;
            if (isHorizontal)
            {
                var limitChecker = row <= board.GetLength(0) && limit <= board.GetLength(1);
                if (limitChecker)
                {
                    for (int i = 0; i < (int)shipType; i++)
                    {
                        var cell = board[row, col + i];
                        hasConflict = cell switch
                        {
                            Types.Empty or Types.Hit => false,
                            _ => true,
                        };
                        if (hasConflict) break;
                    }

                    return !hasConflict;
                }
                return false;
            }
            else
            {
                var limitChecker = limit <= board.GetLength(0) && col <= board.GetLength(1);
                if (limitChecker)
                {
                    for (int i = 0; i < (int)shipType; i++)
                    {
                        var cell = board[row + i, col];
                        hasConflict = cell switch
                        {
                            Types.Empty or Types.Hit => false,
                            _ => true,
                        };
                        if (hasConflict) break;
                    }

                    return !hasConflict;
                }
                
                return false;
            }
        }

        public void Placeship(int rowNum, int colNum, Types shipType, Types[,] board, bool isHorizontal)
        {
            if (isHorizontal)
            {
                for (int i = 0; i < (int)shipType; i++)
                {
                    board[rowNum, colNum + i] = shipType;
                }
            }
            else
            {
                for (int i = 0; i < (int)shipType; i++)
                {
                    board[rowNum + i, colNum] = shipType;
                }
            }
        }

        public bool AttackPlayer(GamePlayer player, int row, int col)
        {
            var hit = player.Board[row -1, col -1] switch
            {
                Types.Empty or Types.Hit => false,
                _ => true,
            };

            if (hit)
            {
                var battleShips = 0;
                player.Board[row - 1, col - 1] = Types.Hit;

                for(int i = 0; i < player.Board.GetLength(0); i++)
                {
                    for (int j = 0; j < player.Board.GetLength(1); j++)
                    {
                        switch(player.Board[i, j])
                        {
                            case Types.Carrier:
                            case Types.Battleship:
                            case Types.Submarine:
                            case Types.Destroyer:
                                battleShips++;
                                break;
                            default:
                                break;
                        }
                    }
                }

                isGameOver = battleShips == 0;
            }

            return hit;
        }
        public Types[,] CreateBoard()
        {
            var board = new Types[boardSize, boardSize];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = Types.Empty;
                }
            }

            return board;
        }

        public void PrintBoard(Types[,] board)
        {
            Console.WriteLine();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write($"[{(int)board[i, j]}]");
                }
                Console.WriteLine();
            }

        }
    }
}
