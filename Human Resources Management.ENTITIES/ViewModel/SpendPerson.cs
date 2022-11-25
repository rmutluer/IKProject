using Human_Resources_Management.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.ViewModel
{
    public class SpendPerson
    {
        public Person Person { get; set; }
        public Expenses Expenses { get; set; }
    }
}
