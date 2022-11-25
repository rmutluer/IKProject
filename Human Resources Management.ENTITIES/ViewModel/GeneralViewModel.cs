using Human_Resources_Management.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.ViewModel
{
    public class GeneralViewModel
    {
        public List<Person> Employees { get; set; }
        public List<Package> Packages{ get; set; }
        public List<Company>  Companies { get; set; }
    }
}
