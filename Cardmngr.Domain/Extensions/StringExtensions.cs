namespace Cardmngr.Domain.Extensions;

public static class StringExtensions
{    
    private readonly static Lazy<char[]> endingChars = new(" \n\r\t<".ToCharArray);

    public static bool TryParseToTagName(this string commentText, out string tagName)
    {
        tagName = null;

        if (string.IsNullOrEmpty(commentText)) return false;

        var firstIndex = commentText.IndexOf('#');

        if (firstIndex == -1) return false;

        var lastIndex = commentText.IndexOfAny(endingChars.Value, firstIndex);

        if (firstIndex >= lastIndex) return false;

        tagName = lastIndex == -1
            ? commentText[(firstIndex + 1)..]
            : commentText[(firstIndex + 1)..lastIndex];

        return true;
    }

    public static string ToCommentContent(this string tagName)
    {
        return $"<p>#{tagName}</p>";
    }
}
