using System.Collections.Concurrent;
using System.Drawing;
using Cardmngr.Domain.Entities;
using Cardmngr.Utils;

namespace Cardmngr.Services;

public class TagColorGetter : ITagColorManager, IDisposable
{
    private readonly IEqualityComparer<TaskTag> nameComparer = new TaskTagNameEqualityComparer();
    private readonly ConcurrentDictionary<string, (Color Color, ConcurrentHashSet<TaskTag> Tags)> colorByTagName = [];
    private readonly Stack<Color> colors = new(
    [
        Color.FromArgb(39, 125, 161),
        Color.FromArgb(87, 117, 144),
        Color.FromArgb(77, 144, 142),
        Color.FromArgb(67, 170, 139),
        Color.FromArgb(144, 190, 109),
        Color.FromArgb(249, 199, 79),
        Color.FromArgb(249, 132, 74),
        Color.FromArgb(248, 150, 30),
        Color.FromArgb(243, 114, 44),
        Color.FromArgb(249, 65, 68),
    ]);

    public Color GetColor(TaskTag tag)
    {
        if (!colorByTagName.TryGetValue(tag.Name, out var data))
        {
            var color = colors.Count > 0 ? colors.Pop() : Color.Transparent;
            colorByTagName[tag.Name] = (color, [tag]);
            return color;
        }

        data.Tags.Add(tag);
        
        return data.Color;
    }

    public void RemoveTag(TaskTag taskTag)
    {
        if (colorByTagName.TryGetValue(taskTag.Name, out var data) && data.Tags.Remove(taskTag) && data.Tags.Count == 0)
        {
            colors.Push(data.Color);
            colorByTagName.TryRemove(taskTag.Name, out _);
        }
    }

    public bool Contains(TaskTag tag) => colorByTagName.ContainsKey(tag.Name) && colorByTagName[tag.Name].Tags.Contains(tag);

    public bool Contains(string tagName) => colorByTagName.ContainsKey(tagName);

    public void Dispose()
    {
        foreach (var (_, data) in colorByTagName)
        {
            data.Tags.Dispose();
        }
    }

    public IEnumerable<TaskTag> Tags => colorByTagName.Values
        .SelectMany(x => x.Tags)
        .Distinct(nameComparer);
}
