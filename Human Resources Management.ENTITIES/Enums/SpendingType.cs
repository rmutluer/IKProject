using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Enums
{
    public enum SpendingType
    {
        [Display(Name = "Konaklama")]
        Konaklama,
        [Display(Name = "Ulaşım")]
        Ulasim,
        [Display(Name = "Yemek")]
        Yemek,
        [Display(Name = "Teşrifat")]
        Tesrifat,
        [Display(Name = "Diğer")]
        Diger
    }
}
