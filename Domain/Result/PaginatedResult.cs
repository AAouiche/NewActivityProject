using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PaginatedResult<T>
{
    public List<T> Items { get; set; }
    public PaginationMetadata Metadata { get; set; }
}