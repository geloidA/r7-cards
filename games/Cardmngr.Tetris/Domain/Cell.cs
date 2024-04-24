namespace Cardmngr.Tetris.Domain;

public class Cell(int row, int column, string? cssClass = null)
{
    public int Row { get; set; } = row;
    public int Column { get; set; } = column;
    public string? CssClass { get; set; } = cssClass;
}
