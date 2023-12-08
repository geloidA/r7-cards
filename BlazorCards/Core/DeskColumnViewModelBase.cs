namespace BlazorCards;

public interface IDeskItem
{
    bool CanMoveNext { get; }
    bool CanMovePrevios { get; }
}

public interface IDeskColumn : IDeskItem
{
    string Title { get; }
    IEnumerable<IDeskColumnCard> Cards { get; }
    int CardCount { get; }
}

public class DeskColumnViewModelBase
{

}
