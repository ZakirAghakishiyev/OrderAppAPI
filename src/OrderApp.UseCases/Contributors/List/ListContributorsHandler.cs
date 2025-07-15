namespace OrderApp.UseCases.Contributors.List;

using Ardalis.SharedKernel;
using Ardalis.Result;
public class ListContributorsHandler(IListContributorsQueryService _query)
  : IQueryHandler<ListContributorsQuery, Result<IEnumerable<ContributorDTO>>>
{
  public async Task<Result<IEnumerable<ContributorDTO>>> Handle(ListContributorsQuery request, CancellationToken cancellationToken)
  {
    var result = await _query.ListAsync();

    return Result.Success(result);
  }
}
