namespace Battleship
{
    public interface IGameService
    {
        void Start();
        void Setup(GamePlayer player);
        void FinishSetup();
        void BattlePhase();
        bool AttackPlayer(GamePlayer player, int row, int col);
        Types[,] CreateBoard();
        void PrintBoard(Types[,] board);
    }
}
