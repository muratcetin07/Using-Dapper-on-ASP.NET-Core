using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Core
{
    public class BaseModel
    {
        
        public int Id { get; set; }

        public BaseModel()
        {
        }
    }
}
