using System;

public class GameState
{
    public enum WinState
    {
        None,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    private readonly byte[,] board = new byte[6, 7];

    public byte PlayerTurn { get; private set; } = 1;
    public int CurrentTurn { get; private set; } = 0;

    public void ResetBoard()
    {
        Array.Clear(board, 0, board.Length);
        PlayerTurn = 1;
        CurrentTurn = 0;
    }

    public byte PlayPiece(byte col)
    {
        if (col > 6)
            throw new ArgumentException("Invalid column.");

        for (int row = 5; row >= 0; row--)
        {
            if (board[row, col] == 0)
            {
                board[row, col] = PlayerTurn;
                byte landingRow = (byte)(6 - row);

                CurrentTurn++;
                PlayerTurn = (byte)(PlayerTurn == 1 ? 2 : 1);

                return landingRow;
            }
        }

        throw new ArgumentException("That column is full.");
    }

    public WinState CheckForWin()
    {
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                byte player = board[row, col];
                if (player == 0)
                    continue;

                if (HasFourHorizontal(row, col, player) ||
                    HasFourVertical(row, col, player) ||
                    HasFourDiagonalDownRight(row, col, player) ||
                    HasFourDiagonalUpRight(row, col, player))
                {
                    return player == 1 ? WinState.Player1_Wins : WinState.Player2_Wins;
                }
            }
        }

        if (CurrentTurn >= 42)
            return WinState.Tie;

        return WinState.None;
    }

    private bool HasFourHorizontal(int row, int col, byte player)
    {
        if (col + 3 >= 7) return false;

        return board[row, col] == player &&
               board[row, col + 1] == player &&
               board[row, col + 2] == player &&
               board[row, col + 3] == player;
    }

    private bool HasFourVertical(int row, int col, byte player)
    {
        if (row + 3 >= 6) return false;

        return board[row, col] == player &&
               board[row + 1, col] == player &&
               board[row + 2, col] == player &&
               board[row + 3, col] == player;
    }

    private bool HasFourDiagonalDownRight(int row, int col, byte player)
    {
        if (row + 3 >= 6 || col + 3 >= 7) return false;

        return board[row, col] == player &&
               board[row + 1, col + 1] == player &&
               board[row + 2, col + 2] == player &&
               board[row + 3, col + 3] == player;
    }

    private bool HasFourDiagonalUpRight(int row, int col, byte player)
    {
        if (row - 3 < 0 || col + 3 >= 7) return false;

        return board[row, col] == player &&
               board[row - 1, col + 1] == player &&
               board[row - 2, col + 2] == player &&
               board[row - 3, col + 3] == player;
    }
}