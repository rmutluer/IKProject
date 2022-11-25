using Human_Resources_Management.ENTITIES.Entities;
using Human_Resources_Management.ENTITIES.ViewModel;
using Human_Resources_Management.REPOSITORIES.Context;
using System.Security.Cryptography;
using Human_Resources_Management.UI.Models;
using HumanResourcesManagement.BUSINESS.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Human_Resources_Management.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericService<Package> _package;
        private readonly IGenericService<Person> _person;
        private readonly IGenericService<Company> _company;
        private readonly HRMDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IGenericService<Package> package, IGenericService<Person> person, IGenericService<Company> company, HRMDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _package = package;
            _person = person;
            _company = company;
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            GeneralViewModel generalViewModel = new GeneralViewModel();
            generalViewModel.Packages = _package.GetAll();
            generalViewModel.Employees = _person.GetAll();
            generalViewModel.Companies = _company.GetAll();
            return View(generalViewModel);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Person userGetpass = _context.Employees.Where(x => x.Mail == email || x.Password==password).FirstOrDefault();
            if (userGetpass==null)
            {
                ViewBag.Hata = "Kullanıcı Adınız veya Şifreniz Yanlıştır.!";
                return View();
            }
            else if (userGetpass != null)
            {
                try
                {
                    var passhased = userGetpass.Password;
                    byte[] hashBytes = Convert.FromBase64String(passhased);
                    byte[] salt = new byte[16];
                    Array.Copy(hashBytes, 0, salt, 0, 16);
                    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);

                    for (int i = 0; i < 20; i++)
                    {
                        try
                        {
                            if (hashBytes[i + 16] != hash[i])
                                throw new UnauthorizedAccessException();
                            else
                            {
                                if (userGetpass != null)
                                {
                                    //TempData["compIdforEmployee"] = userGetID;
                                    var claims = new List<Claim>()
                                       {
                                        new Claim("ID", userGetpass.ID.ToString()),
                                        new Claim(ClaimTypes.Name, userGetpass.FirstName),
                                        new Claim(ClaimTypes.Surname, userGetpass.LastName),
                                        new Claim(ClaimTypes.Email, userGetpass.Mail),
                                        new Claim(ClaimTypes.Role, userGetpass.RoleEnum.ToString()),
                                        new Claim("Photo", userGetpass.PhotoName.ToString())
                                       };
                                    var userIdentity = new ClaimsIdentity(claims, "login");
                                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                                    await HttpContext.SignInAsync(principal);
                                  
                                    if (userGetpass != null)
                                    { if (userGetpass.RoleEnum == ENTITIES.Enums.RoleEnum.Yönetici)
                                        {
                                            return RedirectToAction("Index", "Manager", new { id = userGetpass.ID });
                                        }
                                        else if (userGetpass.RoleEnum == ENTITIES.Enums.RoleEnum.Admin)
                                        {
                                            return RedirectToAction("Index", "Admin", new { id = userGetpass.ID });
                                        }
                                        else if (userGetpass.RoleEnum == ENTITIES.Enums.RoleEnum.Personel)
                                        {
                                            return RedirectToAction("GetPersonel", "Employee", new { id = userGetpass.ID });
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Hata = "Kullanıcı Adınız veya Şifreniz Yanlıştır.!";

                                        return View();
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                            ViewBag.Hata = "Kullanıcı Adınız veya Şifreniz Yanlıştır.!";

                            return View();
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.Hata2 = "Lütfen Tüm Alanları Doldurunuz.";
                }
            }
            else
            {
                ViewBag.Hata2 = "Lütfen Tüm Alanları Doldurunuz.";
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Reset()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Reset(string email)  //buradaki gibi yada yukarıdaki gibi bilgilerimizi alabiliyoruz.
        {
            Person userEntrance = _context.Employees.Where(x => x.Mail == email).FirstOrDefault();
            
            string uniqueName = $"{Guid.NewGuid().ToString().ToLower()}";

            if (userEntrance != null)
            {
                var fromAddress = new MailAddress("i_am_hr@outlook.com");
                var toAddress = new MailAddress(email);
                var Link = "Şifrenizi Yenilemek İçin Linki Tıklayınız<a href= http://imhere.azurewebsites.net/Home/ResetPass/"+email+">Buraya Tıklayınız</a>.";
                //Email yerine Guid veya token ile değişiklik yapılacak
                string resetPass = "Şifre Yenileme Bağlantınız";
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
                        {
                            using (var message = new MailMessage(fromAddress, toAddress) { Subject = resetPass, Body = Link, IsBodyHtml = true })
                            {
                                smtp.Send(message);
                            }
                        }
                        ViewBag.Sonuc = "Mail Başarıyla Gönderildi.";
                    }
                    catch (Exception)
                    {
                        ViewBag.Sonuc = "Mail Gönderiminde Hata Oluştu.";
                    }
            }
            return View();
        }

        public IActionResult ResetPass()
        {
            return View();
        }


         [Route("Home/ResetPass/{email}/")]
        [HttpPost]
        public IActionResult ResetPass(string email, string password, string password2)//token ekle
        {
            ViewBag.userResetEmail = Request.Query["email"];
            Person userEntrance = _context.Employees.Where(x => x.Mail == email).FirstOrDefault();
            if (password.Length >= 8)
            {
                if (userEntrance.Password != password && password == password2)
                {
                    byte[] salt;
                    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    byte[] hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);
                    string savedPasswordHash = Convert.ToBase64String(hashBytes);
                    userEntrance.Password = savedPasswordHash;
                    _person.Update(userEntrance);
                    return RedirectToAction("Login");
                }
                else if(userEntrance==null)
                {
                    ViewBag.Empty = "Email adresiniz sistemde bulunamadı.";
                    return View();
                }
                else
                {
                    ViewBag.Hata1 = " Şifreler uyuşmuyor eski şifre kabul edilemez.";
                    return View();
                }
            }
            ViewBag.Hata = "Şifreniz en az 8 karakterden oluşmalıdır";
            return View();
        }

        public IActionResult DetailsPackage(int id)
        {
            //Package package = _package.GetByID(id);
            ViewBag.Paketler = _package.GetAll();
            foreach (var item in ViewBag.Paketler)
            {
                if (item.ID==id)
                {
                    return View(item);
                }
            }
            return RedirectToAction("Login");
        }
    }
}


