using NUnit.Framework;
using Moq;
using FluentAssertions;
using Battleship;

namespace Battleship.Tests
{
    public class GameServiceTests
    {
        private GameService _gameService;
        [SetUp]
        public void Setup()
        {
            _gameService = new GameService();
        }

        [Test]
        public void CreateBoard_ShouldCreateBoard()
        {
            var res = _gameService.CreateBoard();
            res.Should().NotBeNull();
        }

        [Test]
        public void AttackPlayer_ShouldReturnTrueOnHit()
        {

            var player = new GamePlayer("test", false);
            var col = 1;
            var row = 1;
            player.Board = _gameService.CreateBoard();
            player.Board[row - 1, col -1] = Enums.Types.Carrier;
            var res = _gameService.AttackPlayer(player, row, col);
            res.Should().BeTrue();
        }

        [Test]
        public void AttackPlayer_ShouldReturnFalseOnMiss()
        {

            var player = new GamePlayer("test", false);
            var col = 1;
            var row = 1;
            player.Board = _gameService.CreateBoard();
            player.Board[row, col] = Enums.Types.Carrier;
            var res = _gameService.AttackPlayer(player, row, col);
            res.Should().BeFalse();
        }


    }
}