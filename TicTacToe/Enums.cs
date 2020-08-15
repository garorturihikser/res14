namespace TicTacToe
{
    public enum SquareState
    {
        Empty = 'n',
        X = 'x',
        O = 'o'
    }

    public enum GameState
    {
        XWin,
        OWin,
        Draw,
        InvalidScreen,
        GoingOn
    }
}