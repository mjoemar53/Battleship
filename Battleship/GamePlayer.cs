namespace Battleship
{
    public class GamePlayer
    {
        public readonly Guid Id;
        public string Name { get; set; }
        public Types[,] Board { get; set; }
        public int ShipsPlaced { get; private set; }
        public bool IsAI { get; set; }
        public GamePlayer(string name, bool isAI)
        {
            Id = Guid.NewGuid();
            Board = new Types[0, 0];
            Name = name ?? $"Player {Id}";
            IsAI = isAI;
            ShipsPlaced = 0;
        }

        public void AddShip() => ShipsPlaced++;

        public Types ShipSelectionMenu()
        {
            Types shipType;
            Console.WriteLine();
            Console.WriteLine("Select the type of battleship to be placed.");
            Console.WriteLine($"1 - Carrier - Size: {(int)Types.Carrier}");
            Console.WriteLine($"2 - Battleship - Size: {(int)Types.Battleship}");
            Console.WriteLine($"3 - Submarine - Size: {(int)Types.Submarine}");
            Console.WriteLine($"4 - Destroyer - Size: {(int)Types.Destroyer}");
            Console.WriteLine();
            switch (Console.ReadLine())
            {
                case "1":
                    shipType = Types.Carrier;
                    break;
                case "2":
                    shipType = Types.Battleship;
                    break;
                case "3":
                    shipType = Types.Submarine;
                    break;
                case "4":
                    shipType = Types.Destroyer;
                    break;
                default:
                    Console.WriteLine("Invalid Entry.");
                    shipType = ShipSelectionMenu();
                    break;
            }
            return shipType;
        }

        public int ShipPlacementMenu(string axis, int boardSize)
        {
            Console.WriteLine();
            Console.WriteLine($"Enter the {axis} for ship placement.");
            if (!int.TryParse(Console.ReadLine(), out int result))
            {
                Console.WriteLine("Invalid Entry");
                return ShipPlacementMenu(axis, boardSize);
            }

            if (result > boardSize)
            {
                Console.WriteLine("Invalid Entry");
                return ShipPlacementMenu(axis, boardSize);
            }
            return result;

        }

        public bool ShipOrientationMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Select the ship's oritentation.");
            Console.WriteLine($"1 - Horizontal");
            Console.WriteLine($"2 - Vertical");
            var isHorizontal = Console.ReadLine() switch
            {
                "1" => true,
                "2" => false,
                _ => ShipOrientationMenu(),
            };
            return isHorizontal;
        }

        public Tuple<int,int> Attack(int boardSize)
        {
            if (IsAI)
            {
                Random rand = new();
                return Tuple.Create(rand.Next(1,boardSize), rand.Next(1, boardSize));
            }

            var rowNumber = AttackMenu("row", boardSize);
            var colNumber = AttackMenu("column", boardSize);
            return Tuple.Create(rowNumber, colNumber);
        }

        private int AttackMenu(string axis, int boardSize)
        {
            Console.WriteLine();
            Console.WriteLine($"Enter the {axis} to attack.");
            if (!int.TryParse(Console.ReadLine(), out int result))
            {
                Console.WriteLine("Invalid Entry");
                return AttackMenu(axis, boardSize);
            }

            if (result > boardSize)
            {
                Console.WriteLine("Invalid Entry");
                return AttackMenu(axis, boardSize);
            }
            return result;
        }
    }
}
