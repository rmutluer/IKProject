using Human_Resources_Management.ENTITIES.Entities;
using Human_Resources_Management.ENTITIES.Enums;
using Human_Resources_Management.ENTITIES.ViewModel;
using Human_Resources_Management.REPOSITORIES.Abstract;
using Human_Resources_Management.REPOSITORIES.Context;
using HumanResourcesManagement.BUSINESS.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Human_Resources_Management.UI.Controllers
{
    /// <summary>
    /// marhannuray@gmail.com
    /// marhan123//personel
    /// muhammedkoc10@gmail.com
    /// furkan123//Yönetici
    /// //resul.mutluer@hotmail.com
    /// nuray123//Admin
    /// </summary>

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IGenericRepository<Package> _package;
        private readonly IWebHostEnvironment _env;
        private readonly IGenericService<Person> _person;
        private readonly IGenericRepository<Company> _company;
        private readonly HRMDbContext _context;

        public AdminController(IGenericRepository<Package> package, IWebHostEnvironment env, IGenericService<Person> person, IGenericRepository<Company> company, HRMDbContext context)
        {
            _package = package;
            _env = env;
            _person = person;
            _company = company;
            _context = context;
        }

        public IActionResult Index(int id)
        {
            Person userEntrance = _context.Employees.Where(x => x.ID == id).FirstOrDefault();
            return View(userEntrance);
        }

        [HttpGet]
        public IActionResult CreateManager()
        {
            CreatePerson createManager = new CreatePerson();
            createManager.Companies = _company.GetAll();
            createManager.Person = new Person();
            return View(createManager);
        }

        [HttpPost]
        public IActionResult CreateManager(CreatePerson createPerson)
        {
            var manager = createPerson.Person;
            var tryMail = _context.Employees.Where(x => x.Mail == createPerson.Person.Mail).Select(y => y.Mail).FirstOrDefault();
            if (tryMail != null)
            {
                ViewBag.AllReadyAddedd = "Mail adresi Sistemde Kayıtlı";
                return View();
            }
            else
            {
                if (manager.Photo == null)
                {
                    if (manager.Gender == ENTITIES.Enums.GenderEnum.Erkek)
                    {
                        manager.PhotoName = "\\img\\3586f868_94a2_4fd2_8933_17cd1ff7605e.jpeg";
                    }
                    else
                    {
                        manager.PhotoName = "\\img\\3a172567_ce6c_4e78_964a_6860b74eeff1.jpeg";
                    }
                }
                else
                {
                    manager.PhotoName = SaveThePicture(manager.Photo);
                }
                manager.RoleEnum = RoleEnum.Yönetici;
                var fromAddress = new MailAddress("i_am_hr@outlook.com");
                var toAddress = new MailAddress(manager.Mail);
                var Link = "Şifrenizi Oluşturmak İçin Linke Tıklayınız<a href= http://imhere.azurewebsites.net/Home/ResetPass/" + manager.Mail + ">Buraya Tıklayınız</a>.";

                string resetPass = "Şifre Oluşturma Bağlantınız";
                using (var smtp = new SmtpClient
                {
                    Host = "smtp-mail.outlook.com",
                    /**/
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(fromAddress.Address, "ik-123456")
                })
                    try
                    {
                        using (var message = new MailMessage(fromAddress, toAddress) { Subject = resetPass, Body = Link, IsBodyHtml = true })
                        {
                            smtp.Send(message);
                        }
                        ViewBag.Success = "Mail Başarıyla Gönderildi.";
                    }
                    catch (Exception)
                    {
                        ViewBag.Unsuccess = "Mail Gönderiminde Hata Oluştu.";
                        return View();
                    }
                _person.Add(manager);
                return RedirectToAction("GetManager");
            }
        }


        [HttpGet]
        public IActionResult CreatePackage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePackage(Package package)
        {
            if (package.StartDate < package.EndDate)
            {
                if (ModelState.IsValid)
                {
                    if (package.Photo != null)
                    {
                        package.PhotoName = SaveThePicture(package.Photo);
                    }
                    else
                    {
                        ViewBag.PackageHata = "Fotoğraf Seçiniz";
                        return View();
                    }
                    _package.Add(package);
                    return RedirectToAction("GetPackage");
                }
            }
            else
            {
                ViewBag.Hata = "Başlangıç Tarihi Bitiş Tarihinden Büyük Olamaz.";
            }
            return View();
        }

        public IActionResult GetPackage()
        {
            return View(_package.GetAll());
        }


        public IActionResult GetManager()
        {
            return View(_person.GetAll());
        }


        [HttpGet]
        public IActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCompany(CompanyPackage companies)
        {
            var company = companies.Company;
            if (company.Photo == null)
                company.PhotoName = "\\img\\3586f868_94a2_4fd2_8933_17cd1ff7605e.jpeg";
            else
                company.PhotoName = SaveThePicture(company.Photo);
            _company.Add(company);
            return RedirectToAction("GetCompanies");
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            return View(_company.GetAll());
        }

        private string SaveThePicture(IFormFile img)
        {
            string filePath = Path.Combine(_env.WebRootPath, "img"); // ~/img

            string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{img.ContentType.Split('/')[1]}"; // Benzersiz isim oluşturma. İsimler Guid oluşturulacak. Küçük harf olacak ve - işaretleri yerine _ işareti olacak.

            string newFilePath = Path.Combine(filePath, uniqueName); //~/img/Dosyadı

            using (FileStream fs = new FileStream(newFilePath, FileMode.Create))
            {
                img.CopyTo(fs);
                return newFilePath.Substring(newFilePath.IndexOf("\\img\\")); // burada da dosya yolunun tammaı yerine \img\ kısmını substirng olarak alsın ve return etsin istiyorum. 
            }
        }
    }
}


