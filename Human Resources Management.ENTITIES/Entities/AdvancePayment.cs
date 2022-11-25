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
    public class AdvancePayment: BaseClass
    {
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DefaultValue(typeof(DateTime), "yyyy-MM-dd")]
        [Display(Name = "Avans Talebi Tarihi")]
        public DateTime AvansRequestDate { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Talep Cevabı")]
        public DateTime AvansResponseDate { get; set; }


        [Display(Name = "Avans Açıklaması")]
        public string Description { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Sadece pozitif rakam girişi yapınız")]
        [Display(Name = "Avans")]       
        [Required(ErrorMessage = "Lütfen Avans Tutarını Giriniz")]
        public decimal Avans { get; set; }


        [Display(Name = "Para Birim")]
        public PriceUnit PriceUnit { get; set; }


        [Display(Name = "Onay Durumu")]
        public AproveStatus AproveStatus { get; set; }


        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public Person  Person { get; set; }
    }
}
