﻿using Human_Resources_Management.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.ViewModel
{
    public class VacationPeople
    {
        public List<Person> Person { get; set; }
        public List<Vacation> Vacations { get; set; }
    }
}
