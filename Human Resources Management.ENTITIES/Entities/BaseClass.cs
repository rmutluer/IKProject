using Human_Resources_Management.ENTITIES.Entities;
using Human_Resources_Management.ENTITIES.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES
{


    public class BaseClass
    {
        [Required]
        public int ID { get; set; }

        [Display(Name = "Fotoğraf")]
        public string PhotoName { get; set; }

        [NotMapped]
        [Display(Name = "Fotoğraf")]
        public IFormFile Photo { get; set; }


        [Display(Name = "Aktif Mi")]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
