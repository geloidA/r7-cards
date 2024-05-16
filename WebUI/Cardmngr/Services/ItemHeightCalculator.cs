using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Feedback;

namespace Cardmngr.Services;

public static class ItemHeightCalculator
{
    const int CardPadding = 8;
    const int RowSpace = 20;
    const int MaxTextHeight = 100;

    public static int CalculateHeight(Feedback feedback)
    {        
        var height = CardPadding +
            FeedbackSizes.CreatorSpace +
            FeedbackSizes.BottomRowSpace + 
            FeedbackSizes.DescMarginTop +
            GetTextHeight(feedback.Description, FeedbackSizes.DescRowLength, RowSpace) +
            GetTextHeight(feedback.Title, FeedbackSizes.TitleRowLength, RowSpace);

        return Math.Max(height, FeedbackSizes.MinCardHeight);
    }

    public static int CalculateHeight(OnlyofficeTask task, List<TaskTag>? tags)
    {
        var descHeight = string.IsNullOrEmpty(task.Description) 
            ? TaskSizes.EmptyDescMargin
            : GetTextHeight(task.Description, TaskSizes.DescRowLength, RowSpace);

        var tagsSpace = CalculateTagsHeight(tags);

        var height = CardPadding +
            TaskSizes.TitlePadding + 
            GetTextHeight(task.Title, TaskSizes.TitleRowLength, RowSpace) +
            descHeight +
            tagsSpace +
            (task.Deadline is null ? 0 : RowSpace) +
            (task.MilestoneId is null ? 0 : TaskSizes.MilestoneSpace) +
            (task.Subtasks.Count == 0 ? 0 : TaskSizes.SubtaskSpace) +
            CalculateResponsiblePartHeight(task);

        return Math.Max(height, TaskSizes.MinCardHeight);
    }

    public static int CalculateTagsHeight(List<TaskTag>? tags)
    {
        if (tags is null || tags.Count == 0) return 0;

        var tagsTotalWidth = tags.Sum(x => TaskSizes.TagCommonWidth + x.Name.Length * TaskSizes.TagCharWidth) + 
            (tags.Count - 1) * TaskSizes.TagsPadding;

        var rowCount = (int)Math.Ceiling(tagsTotalWidth / TaskSizes.TagOneRowWidth);

        return TaskSizes.TagRowSpace * rowCount + (rowCount - 1) * TaskSizes.TagsPadding +
            8; // idk why but it's needed
    }

    private static int CalculateResponsiblePartHeight(OnlyofficeTask task)
    {
        var multipleSize = 2 * TaskSizes.ResponsibleSpace + TaskSizes.ResponsibleMoreCountSpace;
        var count = task.Responsibles.Count;

        return TaskSizes.ResponsiblesPadding + CardPadding +
            (count < 3 
            ? count * TaskSizes.ResponsibleSpace 
            : multipleSize);
    }

    private static int GetTextHeight(string? text, float midRowLength, int oneRowSpace)
    {
        if (string.IsNullOrWhiteSpace(text)) return oneRowSpace; // empty text anyway visible

        var height = text
            .Split('\n')
            .Sum(line => CalculateLineHeight(line, midRowLength, oneRowSpace));

        return Math.Min(height, MaxTextHeight);
    }

    private static int CalculateLineHeight(string line, float midRowLength, int oneRowSpace)
    {
        if (string.IsNullOrWhiteSpace(line)) return oneRowSpace;
        return (int)Math.Ceiling(line.Length / midRowLength) * oneRowSpace;
    }

    private static class FeedbackSizes
    {
        internal const int DescMarginTop = 4;
        internal const float DescRowLength = 55f;
        internal const float TitleRowLength = 50f;
        internal const int CreatorSpace = 33 + 8;
        internal const int BottomRowSpace = 20 + 8;
        internal const int MinCardHeight = 100;
        internal const int MaxTextHeight = 100;
    }

    private static class TaskSizes
    {
        internal const int TitlePadding = 8;
        internal const int ResponsibleMoreCountSpace = 20;
        internal const int ResponsibleSpace = 33;
        internal const int ResponsiblesPadding = 8;
        internal const int MinCardHeight = 150;
        internal const int SubtaskSpace = 20 + 8;
        internal const int MilestoneSpace = 20 + 8; 
        internal const int EmptyDescMargin = 48;
        internal const float DescRowLength = 45f;
        internal const float TitleRowLength = 50f;

        internal const int TagRowSpace = 28;
        internal const int TagsPadding = 4; // padding between tags
        internal const int TagCommonWidth = 27 + 8; // tag icon + 8px padding
        internal const int TagCharWidth = 7;
        internal const float TagOneRowWidth = 328;
    }
}
