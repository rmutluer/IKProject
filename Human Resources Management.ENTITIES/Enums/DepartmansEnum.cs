using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Enums
{
    public enum DepartmansEnum
    {
        [Display(Name = "Yazılım")]
        Yazilim,
        [Display(Name = "Muhasebe")]
        Muhasabe,
        [Display(Name = "İnsan Kaynakları")]
        IK,
        [Display(Name = "Yönetim")]
        Yönetim,
        [Display(Name = "Müdür")]
        Müdür,
        [Display(Name = "Satış Pazarlama")]
        Satış,
        [Display(Name = "IT")]
        IT
    }
}
