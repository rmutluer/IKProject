using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Enums
{
    public enum AproveStatus
    {
        [Display(Name ="Onay Bekliyor")]
        OnayBekliyor,
        [Display(Name = "Onaylandı")]
        Onaylandi,
        [Display(Name = "Onaylanmadı")]
        Onaylanmadi
    }
}
