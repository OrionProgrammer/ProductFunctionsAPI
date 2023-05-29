using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFunctionsAPI.Models;

public class ProductModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString("n");

    public string Name { get; set; }

    public string Description { get; set; }

    public bool InStock { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}

