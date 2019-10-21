using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class User : Core.BaseModel
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Address { get; set; }
    }
}
