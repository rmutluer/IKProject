using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Enums
{
    public enum VacationType
    {
        [Display(Name = "Yıllık")]
        Yillik,
        [Display(Name = "Doğum")]
        Dogum,
        [Display(Name = "Babalık")]
        Babalik,
        [Display(Name = "Evlilik")]
        Evlilik
    }
}
