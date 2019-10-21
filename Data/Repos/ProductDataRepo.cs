using Data.Abstract;
using Data.Concrete;
using Data.Core;
using Microsoft.Extensions.Configuration;
using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Data.Repos
{
    public class ProductDataRepo : DapperSqlServerBaseData<Product>, IProductDataRepo
    {
        public ProductDataRepo(IConfiguration config) : base(config)
        {
        }
    }
}
