namespace Cardmngr.Shared.Utils.Comparer;

public interface IEqualityComparer<T1, T2>
{
    bool Equals(T1? x, T2? y);
    int GetHashCode(T1? obj);
    int GetHashCode(T2? obj);
}
