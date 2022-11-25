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
    public class Vacation : BaseClass
    {
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DefaultValue(typeof(DateTime), "yyyy-MM-dd")]
        [Display(Name = "İzin Başlangıç Tarihi")]
        public DateTime VacationStartDate { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DefaultValue(typeof(DateTime), "yyyy-MM-dd")]
        [Display(Name = "İzin Bitiş Tarihi")]
        public DateTime VacationEndDate { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DefaultValue(typeof(DateTime), "yyyy-MM-dd")]
        [Display(Name = "İzin Talebi Tarihi")]
        public DateTime VacationRequestDate { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Talep Cevabı")]
        public Nullable<DateTime> VacationResponseDate { get; set; }


        [Display(Name = "Onay Durumu")]
        public AproveStatus AproveStatus { get; set; }


        [Display(Name = "İzin Türü")]
        public VacationType VacationType { get; set; }


        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public Person Person { get; set; }
    }
}
