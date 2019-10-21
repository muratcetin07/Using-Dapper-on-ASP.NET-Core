using Model.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Product : BaseModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
