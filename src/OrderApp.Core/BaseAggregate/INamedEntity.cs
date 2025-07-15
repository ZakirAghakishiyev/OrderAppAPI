using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApp.Core.BaseAggregate;

public interface INamedEntity
{
    public string Name { get; set; }

}
