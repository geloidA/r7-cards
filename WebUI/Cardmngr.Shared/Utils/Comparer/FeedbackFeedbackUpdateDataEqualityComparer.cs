using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Shared.Feedbacks;

namespace Cardmngr.Shared.Utils.Comparer;

public class FeedbackFeedbackUpdateDataEqualityComparer : IEqualityComparer<Feedback, FeedbackUpdateData>
{
    public bool Equals(Feedback? x, FeedbackUpdateData? y)
    {
        if (x == null || y == null) return false;
        return x.Title == y.Title &&
            x.Description == y.Description;
    }

    public int GetHashCode(Feedback? obj)
    {
        if (obj == null) return 0;
        return obj.GetHashCode();
    }

    public int GetHashCode(FeedbackUpdateData? obj)
    {
        if (obj == null) return 0;
        return obj.GetHashCode();
    }
}
