
using System.Collections;

namespace BlazorCards.Core;

public interface ICardTrack : IUIElement, IEnumerable<CardBase>
{
    string? Title { get; set; }
    int Count { get; }
    void AddAfter(CardBase target, CardBase item);
    void AddBefore(CardBase target, CardBase item);
    void AddFirst(CardBase item);
    void AddLast(CardBase item);
    void RemoveLast();
    void RemoveFirst();
    void Remove(CardBase item);
    CardBase? First { get; }
    CardBase? Last { get; }
}

public abstract class CardBase : IUIElement
{
    public string? Title { get; set; }
    public string? Description { get; set; }

    public ICardTrack? Track { get; private set; }

    public abstract string CssName { get; }

    public abstract string CssColor { get; }

    public event Action<CardBase>? DragStart;

    public void OnDragStarted()
    {
        DragStart?.Invoke(this);
    }

    public abstract class CardTrackViewModelBase : ICardTrack
    {
        protected LinkedList<CardBase> cards = new();

        public CardTrackViewModelBase() { }

        public CardTrackViewModelBase(IEnumerable<CardBase> items)
        {
            foreach (var item in items)
            {
                item.Track = this;
            }
            
            cards = new LinkedList<CardBase>(items);
        }

        public string? Title { get; set; }

        public int Count => cards.Count;

        public abstract string CssName { get; }

        public abstract string CssColor { get; }

        public CardBase? First => cards.First?.Value;

        public CardBase? Last => cards.Last?.Value;

        public void AddAfter(CardBase target, CardBase item)
        {
            cards.AddAfter(cards.Find(target) ?? throw new InvalidOperationException(), item);
            item.Track?.Remove(item);
            item.Track = this;
        }

        public void AddBefore(CardBase target, CardBase item)
        {
            cards.AddBefore(cards.Find(target) ?? throw new InvalidOperationException(), item);
            item.Track?.Remove(item);
            item.Track = this;
        }

        public void AddFirst(CardBase item)
        {
            cards.AddFirst(item);
            item.Track?.Remove(item);
            item.Track = this;
        }

        public void AddLast(CardBase item)
        {
            cards.AddLast(item);
            item.Track?.Remove(item);
            item.Track = this;
        }
        
        public IEnumerator<CardBase> GetEnumerator() => cards.GetEnumerator();

        public void Remove(CardBase item)
        {
            Console.WriteLine("Removed: " + item.Track?.Count);
            if (cards.Remove(item))
            {
                Console.WriteLine("Removed: " + item.Track!.Count);
            }
            item.Track = null;
        }

        public void RemoveFirst()
        {
            if (cards.First is { })
            {
                cards.First.Value.Track = null;
            }

            cards.RemoveFirst();
        }

        public void RemoveLast()
        {
            if (cards.Last is { })
            {
                cards.Last.Value.Track = null;
            }

            cards.RemoveLast();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}