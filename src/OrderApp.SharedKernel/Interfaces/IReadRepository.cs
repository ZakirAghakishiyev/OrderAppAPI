using Ardalis.Specification;

namespace OrderApp.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot { }
