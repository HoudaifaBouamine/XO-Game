using xo.Api.Dtos.GameDtos;

namespace xo.Api.Entities
{
    public partial class Game
    {

        public GameReadDto ToDto(IEnumerable<Player> players)
        {
            return new GameReadDto()
            {
                Game_Id = this.Game_Id,
                Board = this.Board,
                Player1 = players.Where(p => p.Player_Id == this.Player1_Id).FirstOrDefault()!,
                Winner = (this.Winner_Id is null) ? null : players.Where(p => p.Player_Id == this.Winner_Id).FirstOrDefault()!,
                CurrentTurn = players.Where(p => p.Player_Id == this.CurrentTurn_Id).FirstOrDefault()!,
                Player2 = players.Where(p => p.Player_Id == this.Player2_Id).FirstOrDefault()!,
                IsGameOver = this.IsGameOver
            };
        }

        public enum enWinner { GameOnGoing = 0, X_Win = 1, O_Win = 2, Draw = -1 }
        static public enWinner CheckWinner(string board)
        {
            // Check rows, columns, and diagonals for a winner
            for (int i = 0; i < 3; i++)
            {
                if (CheckLine(board[i * 3], board[i * 3 + 1], board[i * 3 + 2]) != 0)
                    return CheckLine(board[i * 3], board[i * 3 + 1], board[i * 3 + 2]); // Check rows

                if (CheckLine(board[i], board[i + 3], board[i + 6]) != 0)
                    return CheckLine(board[i], board[i + 3], board[i + 6]); // Check columns
            }

            if (CheckLine(board[0], board[4], board[8]) != 0)
                return CheckLine(board[0], board[4], board[8]); // Check main diagonal

            if (CheckLine(board[2], board[4], board[6]) != 0)
                return CheckLine(board[2], board[4], board[6]); // Check anti-diagonal

            // Check for a draw
            if (!board.Contains("#"))
                return enWinner.Draw;

            // No winner yet
            return enWinner.GameOnGoing;
        }

        static enWinner CheckLine(char a, char b, char c)
        {
            if (a == 'X' && b == 'X' && c == 'X')
                return enWinner.X_Win; // Player X wins
            if (a == 'O' && b == 'O' && c == 'O')
                return enWinner.O_Win; // Player O wins
            return enWinner.GameOnGoing; // No winner in this line
        }
    }

}
