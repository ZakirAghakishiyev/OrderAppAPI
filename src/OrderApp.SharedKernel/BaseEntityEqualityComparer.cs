using System.Diagnostics.CodeAnalysis;

namespace OrderApp.SharedKernel;

public class BaseEntityEqualityComparer<T> : IEqualityComparer<T> where T : BaseEntity
{
    public bool Equals(T? x, T? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode([DisallowNull] T obj) => obj.Id.GetHashCode();
}
