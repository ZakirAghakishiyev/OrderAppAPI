using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using OrderApp.SharedKernel;

namespace OrderApp.Core.BaseAggregate;

public abstract class NamedEntity : AuditedSoftDeletedEntity, INamedEntity
{
    [Column(Order = 2)]
    public string Name { get; set; } = null!;

}
