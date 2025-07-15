using Ardalis.Specification;

namespace OrderApp.SharedKernel;

public class ListPageSpec<T> : Specification<T>
{
    public ListPageSpec(int perPage, int page)
    {
        Query.Skip(perPage * (page - 1)).Take(perPage);
    }
}
