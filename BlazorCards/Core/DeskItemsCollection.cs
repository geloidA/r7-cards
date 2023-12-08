using System.Collections;

namespace BlazorCards;

public interface IDeskItemsCollection : IEnumerable<IDeskItem>
{
    bool Contains(IDeskItem deskItem);
    void MoveNext(IDeskItem deskItem);
    void MovePrevios(IDeskItem deskItem);
}

public class DeskItemsCollection : IDeskItemsCollection
{
    private readonly List<IDeskItem> deskItems = [];

    public bool Contains(IDeskItem deskItem)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<IDeskItem> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void MoveNext(IDeskItem deskItem)
    {
        throw new NotImplementedException();
    }

    public void MovePrevios(IDeskItem deskItem)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}