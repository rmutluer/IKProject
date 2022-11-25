using Human_Resources_Management.ENTITIES.Entities.CustomValidation;
using Human_Resources_Management.ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Entities
{

    public class Package : BaseClass
    {
        [Display(Name = "Paket Adı")]
        [Required(ErrorMessage = "Lütfen Paket Adı Giriniz")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage ="Sadece Harf Girişi Yapınız")]
        public string PackageName { get; set; }


        [NoNegative(ErrorMessage = "Negatif Değer Olamaz")]
        [Required(ErrorMessage = "Lütfen Kullanıcı Sayısı Giriniz")]
        [Display(Name = "Kullanıcı Sayısı")]
        public int NumberOfUsers { get; set; }


        [NoNegative(ErrorMessage = "Negatif Değer Olamaz")]
        [Required(ErrorMessage = "Lütfen Geçerlilik Süresi Belirtiniz")]
        [Display(Name = "Geçerlilik Süresi")]
        public int ValidityDay { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [BiggerThanToday]
        [Required(ErrorMessage = "Başlangıç Tarihi Boş Geçilemez")]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [BiggerThanToday]
        [Column(TypeName = "date")]
        [Display(Name = "Bitiş Tarihi")]
        [Required(ErrorMessage = "Bitiş Tarihi Boşgeçilemez")]
        public DateTime EndDate { get; set; }
    
        [Display(Name = "Fiyatı")]
        [PriceValidation(ErrorMessage = "Maaş Değeri Negatif Olamaz")]
        [Required(ErrorMessage = "Fiyat Boş geçilemez")]
        public decimal Price { get; set; }


        [Display(Name = "Para Birim")]
        [Required(ErrorMessage = "Para Birimi Boş geçilemez")]
        public PriceUnit PriceUnit { get; set; }


        [Display(Name = "Açıklaması")]
        [Required(ErrorMessage = "Lütfen Açıklama Giriniz")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "3 ila 250 Karakter arası")]
        public string Description { get; set; }
        public virtual List<Company> Company { get; set; }
    }
}
