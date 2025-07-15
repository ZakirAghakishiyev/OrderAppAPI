using System.ComponentModel.DataAnnotations.Schema;

namespace OrderApp.SharedKernel;

public abstract class BaseEntity
{
    [Column(Order = 1)]
    public int Id { get; set; }
}
