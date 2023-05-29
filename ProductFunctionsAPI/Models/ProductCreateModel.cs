using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFunctionsAPI.Models;

public class ProductCreateUpdateModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public bool InStock { get; set; }
}

