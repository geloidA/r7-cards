using System.Drawing;
using Cardmngr.Domain.Entities;

namespace Cardmngr.Services;

public interface ITagColorManager
{
    Color GetColor(TaskTag tag);

    void RemoveTag(TaskTag tag);

    bool Contains(string tagName);

    IEnumerable<TaskTag> Tags { get; }
}
