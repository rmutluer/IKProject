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
    [ModelMetadataType(typeof(ManagerDataTypes))]
    public class Person : BaseClass
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Lütfen Adınızı Giriniz")]        
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Lütfen Soyadınızı Giriniz")]     
        public string LastName { get; set; }

        [Display(Name = "İkinci Adı")]
        public string MiddleName { get; set; }

        [Display(Name = "İkinci Soyadı")]       
        public string MaidenName { get; set; }

        [Display(Name = "Departman")]
        [Required]
        public DepartmansEnum DepartmansEnum { get; set; }

        [Display(Name = "Meslek")]      
        public string Job { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "İşe Giriş Tarihi")]
        public Nullable<DateTime> HireDate { get; set; }

        [Display(Name = "İşten Çıkış Tarihi")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public Nullable<DateTime> LeaveJobDate { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        [Column(TypeName ="date")]
        [Required(ErrorMessage = "Lütfen doğum tarihinizi giriniz")]
        [CantHireYet]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Lütfen Adresinizi Giriniz")]      
        public string Address { get; set; }

        [Display(Name = "Mail")]
        public string Mail { get; set; }

        [Display(Name = "Şifre")]       
        public string Password { get; set; }

        [ForeignKey("Employees")]
        public int? ReportsTo { get; set; }
        public virtual Person Employees { get; set; }


        [Display(Name = "Telefon")]    
        public string PersonelPhoneNumber { get; set; }

        [Display(Name = "Şirket Telefonu")]       
        public string CompanyPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Cinsiyet")]
        public GenderEnum Gender { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public RoleEnum RoleEnum { get; set; }

        [Required]
        [Display(Name = "Maaş")]       
        public decimal Salary { get; set; }

        [ForeignKey("Company")]
        public int? CompanyID { get; set; }
        public virtual Company Company { get; set; }

        public List<Expenses> Expenses { get; set; }
        public List<AdvancePayment> AdvancePayments { get; set; }
        public List<Vacation> Vacations { get; set; }
    }
}
