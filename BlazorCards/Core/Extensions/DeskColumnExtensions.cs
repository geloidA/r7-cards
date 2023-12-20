namespace BlazorCards.Core.Extensions;

public static class DeskColumnExtensions
{
    public static IEnumerable<Card> AllCards(this IEnumerable<Board> boards)
    {
        return boards
            .SelectMany(x => x)
            .SelectMany(x => x);
    }
}
