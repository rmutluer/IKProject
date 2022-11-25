using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Entities
{
    public class Company :BaseClass
    {
        [Display(Name = " Şirket Adı")]
        [Required(ErrorMessage = "Lütfen Şirket Adını Giriniz")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3 ila 50 Karakter arası")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage = "Sadece Harf Girişi Yapınız")]
        public string CompanyName { get; set; }


        [Display(Name = "Unvanı")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3 ila 50 Karakter arası")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage = "Sadece Harf Girişi Yapınız")]
        public string CorporateName { get; set; }


        [Display(Name = "Şirket Adresi")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "3 ila 100 Karakter arası")]
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Mail adresi boş bırakılamaz")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Mail Formatını Doğru Giriniz")]
        [Display(Name = "Şirket Maili")]
        public string CompanyMail { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Lütfen Telefon Numaranızı Giriniz")]
        [Display(Name = "Şirket Numarası")]
        public string CompanyPhoneNumber { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Sadece pozitif rakam girişi yapınız")]
        [Display(Name = "Vergi Numarası")]
        public string TaxNumber { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "3 ila 100 Karakter arası")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage = "Sadece Harf Girişi Yapınız")]
        [Display(Name = "Vergi Dairesi")]
        public string TaxAdministration { get; set; }


        [StringLength(100, MinimumLength = 3, ErrorMessage = "3 ila 100 Karakter arası")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage = "Sadece Harf Girişi Yapınız")]
        [Display(Name = "Şirket Türü")]
        public string KindOfCorporation { get; set; }

        public virtual List<Person> People { get; set; }

        [ForeignKey("Package")]
        public int? PackageID { get; set; }
        public Package Package { get; set; }
    }
}
