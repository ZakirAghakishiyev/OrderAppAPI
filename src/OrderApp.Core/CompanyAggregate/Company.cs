using Ardalis.SharedKernel;
using SH=OrderApp.SharedKernel.Interfaces;

namespace OrderApp.Core.CompanyAggregate;


public class Company: SH.IAggregateRoot
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

}


