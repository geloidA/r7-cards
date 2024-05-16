using Cardmngr.Domain.Feedback;

namespace Cardmngr.Services;

public class ItemHeightCalculator
{
    public static int CalculateFeedbackHeight(Feedback feedback)
    {        
        var height = FeedbackConstants.CardPadding +
            FeedbackConstants.CreatorSpace +
            FeedbackConstants.BottomRowSpace + 
            FeedbackConstants.DescMarginTop +
            GetTextHeight(feedback.Description, FeedbackConstants.DescRowLength, FeedbackConstants.RowSpace) +
            GetTextHeight(feedback.Title, FeedbackConstants.TitleRowLength, FeedbackConstants.RowSpace);

        return Math.Max(height, FeedbackConstants.MinCardHeight);
    }

    private static int GetTextHeight(string? text, float midRowLength, int oneRowSpace)
    {
        if (string.IsNullOrWhiteSpace(text)) return oneRowSpace; // empty text anyway visible

        var height = text
            .Split('\n')
            .Sum(line => CalculateLineHeight(line, midRowLength, oneRowSpace));

        return Math.Min(height, FeedbackConstants.MaxTextHeight);
    }

    private static int CalculateLineHeight(string line, float midRowLength, int oneRowSpace)
    {
        if (string.IsNullOrWhiteSpace(line)) return oneRowSpace;
        return (int)Math.Ceiling(line.Length / midRowLength) * oneRowSpace;
    }

    private static class FeedbackConstants
    {
        internal const int CardPadding = 8;
        internal const int DescMarginTop = 4;
        internal const int RowSpace = 20;
        internal const float DescRowLength = 55f;
        internal const float TitleRowLength = 50f;
        internal const int CreatorSpace = 33 + 8;
        internal const int BottomRowSpace = 20 + 8;
        internal const int MinCardHeight = 100;
        internal const int MaxTextHeight = 100;
    }
}
