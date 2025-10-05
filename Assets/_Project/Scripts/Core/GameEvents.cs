using System;

public enum GameOverReason
{
    OutOfMoves,
    WokeUp
}

public static class GameEvents
{
    // Fired when the player makes a move (current moves, max moves)
    public static Action<int, int> OnMovesChanged;

    // Fired when the level is won
    public static Action OnWin;

    // Fired when the level is lost (reason: out of moves or woke up)
    public static Action<GameOverReason> OnGameOver;
}
