namespace Cardmngr.Tetris.Domain;

public class CellCollection
{
    private readonly List<Cell> cells = [];

    public bool Contains(int row, int column) => cells.Any(x => x.Row == row && x.Column == column);

    public bool HasColumn(int column) => cells.Any(x => x.Column == column);
    
    public bool HasRow(int row) => cells.Any(x => x.Row == row);

    public void Add(int row, int column) => cells.Add(new Cell(row, column));

    public void AddMany(List<Cell> cells, string cssClass) => cells.ForEach(c => this.cells.Add(new Cell(c.Row, c.Column, cssClass)));

    public List<Cell> GetAll() => cells;

    public void SetCssClass(int row, string cssClass) => cells
        .Where(x => x.Row == row)
        .ToList()
        .ForEach(x => x.CssClass = cssClass);

    public void CollapseRows(List<int> rows)
    {
        cells.RemoveAll(x => rows.Contains(x.Row));
    }
}
