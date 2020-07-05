using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Photos.Shared.Models.Entities
{
    public class PhoneEntity : TableEntity
    {
        
        public string PhoneNumber { get; set; }
    }
}
