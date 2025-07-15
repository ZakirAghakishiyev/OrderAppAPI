using SKInterface=OrderApp.SharedKernel.Interfaces;
using Org.BouncyCastle.Crypto.Digests;
using SK=OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T>(AppDbContext dbContext) :
  RepositoryBase<T>(dbContext), SKInterface.IReadRepository<T>, SK.IRepository<T> where T : class, SKInterface.IAggregateRoot
{
}
