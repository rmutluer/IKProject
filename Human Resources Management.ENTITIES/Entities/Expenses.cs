using Human_Resources_Management.ENTITIES.Entities.CustomValidation;
using Human_Resources_Management.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Entities
{
    public class Expenses : BaseClass
    {
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DefaultValue(typeof(DateTime), "yyyy-MM-dd")]
        [Display(Name = "Harcama Talebi Tarihi")]
        public DateTime SpendingRequestDate { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Talep Cevabı")]
        public Nullable<DateTime> SpendingResponseDate { get; set; }


        [Display(Name = "Harcama")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Sadece pozitif rakam girişi yapınız")]
        [Required(ErrorMessage = "Lütfen Harcama Tutarını Giriniz")]
        public decimal Spending { get; set; }

        [Display(Name = "Harcama Türü")]
        public SpendingType SpendingType { get; set; }

        [Display(Name = "Para Birim")]
        public PriceUnit PriceUnit { get; set; }


        [Display(Name = "Onay Durumu")]
        public AproveStatus AproveStatus { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public Person Person { get; set; }
    }
}
