using Human_Resources_Management.ENTITIES.Entities;
using Human_Resources_Management.ENTITIES.ViewModel;
using Human_Resources_Management.REPOSITORIES.Context;
using HumanResourcesManagement.BUSINESS.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Human_Resources_Management.UI.Controllers
{
    [Authorize(Roles = "Yönetici")]
    public class ManagerController : Controller
    {
        private readonly IGenericService<Person> _person;
        private readonly IWebHostEnvironment _env;
        private readonly HRMDbContext _context;
        private readonly IGenericService<Company> _company;
        private readonly IGenericService<Expenses> _expenses;
        private readonly IGenericService<AdvancePayment> _advance;
        private readonly IGenericService<Vacation> _vacation;

        public ManagerController(IGenericService<Person> person, IWebHostEnvironment env, HRMDbContext context, IGenericService<Company> company, IGenericService<Expenses> expenses, IGenericService<AdvancePayment> advance, IGenericService<Vacation> vacation)
        {
            _person = person;
            _env = env;
            _context = context;
            _company = company;
            _expenses = expenses;
            _advance = advance;
            _vacation = vacation;
        }
        //  [Route("Manager/Index/{id}")]
        public IActionResult Index(int id)
        {
            Person userEntrance = _context.Employees.Where(x => x.ID == id).FirstOrDefault();
            return View(_person.GetByID(id));
        }

        public IActionResult Details(int id)
        {
            return View(_person.GetByID(id));
        }
        //  [Route("Manager/Update/{id}")]
        public IActionResult Update(int id)
        {
            return View(_person.GetByID((int)id));
        }

        // [Route("Manager/Update/{id}")]
        [HttpPost]
        public IActionResult Update([FromForm] Person person, int id)
        {
            Person managerUpdated = _person.GetByID(id);
            managerUpdated.Address = person.Address;
            managerUpdated.PersonelPhoneNumber = person.PersonelPhoneNumber;
            if (person.Photo != null)
            {
                managerUpdated.PhotoName = ResimKaydet(person.Photo);
            }
            _person.Update(managerUpdated);
            return RedirectToAction(nameof(Index), new { id = id });
        }



        public string ResimKaydet(IFormFile img)
        {
            string filePath = Path.Combine(_env.WebRootPath, "img"); // ~/img
            string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{img.ContentType.Split('/')[1]}";
            // Benzersiz isim oluşturma. İsimler Guid oluşturulacak. Küçük harf olacak ve - işaretleri yerine _ işareti olacak.
            string newFilePath = Path.Combine(filePath, uniqueName); //~/img/Dosyadı
                                                                     //string uniqeName = Guid.NewGuid().ToString() +"_"+img.FileName ;
                                                                     //string path = Path.Combine(_env.WebRootPath, "img",uniqeName);
            using (FileStream fs = new FileStream(newFilePath, FileMode.Create))
            {
                img.CopyTo(fs);
                return newFilePath.Substring(newFilePath.IndexOf("\\img\\"));
                // burada da dosya yolunun tammaı yerine \img\ kısmını substirng olarak alsın ve return etsin istiyorum. 
                //NOT: 2 adet \ işaretinden biri kaçış karakteri. Ama renkleri düzgün göremiyorum :)<3 gayet iyi duruyoD:
            }
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            CreatePerson createEmployee = new CreatePerson();
            createEmployee.Companies = _company.GetAll();
            createEmployee.Person = new Person();
            List<Person> Managers = _context.Employees.Where(x => x.RoleEnum == ENTITIES.Enums.RoleEnum.Yönetici).ToList();

            ViewBag.managers = Managers;
            return View(createEmployee);
        }


        [HttpPost]
        public IActionResult CreateEmployee(Person person, int id)
        {
            var managerComp = _person.GetByDefault(x => x.ID == id);           
            var employeeCompId = managerComp.CompanyID;
            var getEmail = _person.GetAll();
            var tryMail = _context.Employees.Where(x => x.Mail == person.Mail).Select(y=>y.Mail).FirstOrDefault();
                if (tryMail !=null)
                {
                    ViewBag.AllReadyAdded = "Mail adresi Sistemde Kayıtlı";
                    return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        if (person.Photo == null)
                        {
                            if (person.Gender == ENTITIES.Enums.GenderEnum.Erkek)
                            {
                                person.PhotoName = "\\img\\3586f868_94a2_4fd2_8933_17cd1ff7605e.jpeg";
                            }
                            else
                            {
                                person.PhotoName = "\\img\\3a172567_ce6c_4e78_964a_6860b74eeff1.jpeg";
                            }
                        }
                        else
                        {
                            person.PhotoName = ResimKaydet(person.Photo);
                        }

                        person.CompanyID = employeeCompId;
                        person.Company = managerComp.Company;
                        person.IsActive = true;
                        person.RoleEnum = ENTITIES.Enums.RoleEnum.Personel;
                        person.ReportsTo = id;
                        _person.Add(person);

                        var fromAddress = new MailAddress("i_am_hr@outlook.com");
                        var toAddress = new MailAddress(person.Mail);
                        var Link = "Şifrenizi Oluşturmak İçin Linke Tıklayınız<a href= http://imhere.azurewebsites.net/Home/ResetPass/" + person.Mail + ">Buraya Tıklayınız</a>.";

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
                                ViewBag.SonucPozitif = "Mail Başarıyla Gönderildi.";
                            }
                            catch (Exception)
                            {
                                ViewBag.SonucNegatif = "Mail Gönderiminde Hata Oluştu.";
                            }
                        return RedirectToAction("GetAllPersonel", "Manager", new { id = id });
                    }
                }
            return View();
        }


        public IActionResult GetPaymentList(int id)
        {
            PeoplePayment ps = new PeoplePayment();
            ps.AdvancePayments = _context.AdvancePayments.Where(x => x.PersonID == x.Person.ID && x.Person.ReportsTo == id).ToList();
            ps.Person = _context.Employees.Where(x => x.ReportsTo == id).ToList();
            return View(ps);
        }

        public IActionResult GetSpendingList(int id)
        {
            PersonSpending ps = new PersonSpending();
            ps.Expenses = _context.Expenses.Where(x => x.PersonID == x.Person.ID && x.Person.ReportsTo == id).ToList();
            ps.Person = _context.Employees.Where(x => x.ReportsTo == id).ToList();
            return View(ps);
        }


        public JsonResult GetPaymentList2(int id)
        {
            var advance = _context.AdvancePayments.Include("Person").Where(x => x.PersonID == x.Person.ID && x.Person.ReportsTo == id).OrderByDescending(x => x.ID).ToList();

            var advancesList = new List<AdvancePayment>();

            foreach (var e in advance)
            {
                Person p = new Person()
                {
                    FirstName = e.Person.FirstName,
                    LastName = e.Person.LastName
                };

                advancesList.Add(new AdvancePayment()
                {
                    Person = p,
                    AproveStatus = e.AproveStatus,
                    Photo = e.Photo,
                    PhotoName = e.PhotoName,
                    PriceUnit = e.PriceUnit,
                    Avans= e.Avans,
                    AvansRequestDate = e.AvansRequestDate,
                    Description=e.Description
                    
                });
            }
            return Json(advancesList);
        }

        [HttpGet]
        public JsonResult GetSpendingList2(int id)
        {
            var expenses = _context.Expenses.Include("Person").Where(x => x.PersonID == x.Person.ID && x.Person.ReportsTo == id).OrderByDescending(x => x.ID).ToList();

            var expensesList = new List<Expenses>();

            foreach (var e in expenses)
            {
                Person p = new Person()
                {
                    FirstName = e.Person.FirstName,
                    LastName = e.Person.LastName
                };

                expensesList.Add(new Expenses()
                {
                    Person = p,
                    AproveStatus = e.AproveStatus,
                    Photo = e.Photo,
                    PhotoName = e.PhotoName,
                    PriceUnit = e.PriceUnit,
                    Spending = e.Spending,
                    SpendingRequestDate = e.SpendingRequestDate,
                    SpendingType = e.SpendingType
                });
            }
            return Json(expensesList);
        }

        [HttpGet]
        public IActionResult Approval(int id)
        {
            Expenses ex = _expenses.GetByID(id);
            return View(ex);
        }

        [HttpPost]
        public IActionResult Approval(Expenses expenses,int id)
        {
            int idimmm =Convert.ToInt32(_context.Expenses.Where(x=>x.PersonID==x.Person.ID).Select(y=>y.Person.ReportsTo).FirstOrDefault());
            Expenses ex = _expenses.GetByID(id);
            ex.AproveStatus = expenses.AproveStatus;
            ex.SpendingResponseDate = DateTime.Now.Date;
            _expenses.Update(ex);
            return RedirectToAction("GetSpendingList", new { id = idimmm });
        }


        [HttpGet]
        public IActionResult ApprovalPayment(int id)
        {
            AdvancePayment ex = _advance.GetByID(id);
            return View(ex);
        }

        [HttpPost]
        public IActionResult ApprovalPayment(Expenses expenses, int id)
        {
            int idimmm = Convert.ToInt32(_context.Expenses.Where(x => x.PersonID == x.Person.ID).Select(y => y.Person.ReportsTo).FirstOrDefault());
            AdvancePayment ex = _advance.GetByID(id);
            ex.AproveStatus = expenses.AproveStatus;
            ex.AvansResponseDate = DateTime.Now.Date;
            _advance.Update(ex);
            return RedirectToAction("GetPaymentList", new { id = idimmm });
        }



        [HttpGet]
        public IActionResult ApprovalVacation(int id)
        {
            Vacation vacation = _vacation.GetByID(id);
            return View(vacation);
        }

        [HttpPost]
        public IActionResult ApprovalVacation(Vacation vacation, int id)
        {
            int idimmm = Convert.ToInt32(_context.Vacations.Where(x => x.PersonID == x.Person.ID).Select(y => y.Person.ReportsTo).FirstOrDefault());
            Vacation vaca = _vacation.GetByID(id);
            vaca.AproveStatus = vacation.AproveStatus;
            vaca.VacationResponseDate = DateTime.Now.Date;
            _vacation.Update(vaca);
            return RedirectToAction("GetVacationList", new { id = idimmm });
        }


        public IActionResult GetVacationList(int id)
        {
            VacationPeople vp = new VacationPeople();
            vp.Vacations = _context.Vacations.Where(x => x.PersonID == x.Person.ID && x.Person.ReportsTo == id).ToList();
            vp.Person = _context.Employees.Where(x => x.ReportsTo == id).ToList();
            return View(vp);
        }

        [HttpGet]
        public IActionResult GetAllPersonel(int id)
        {
            var managerComp = _context.Employees.Where(x=>x.ID==id).Select(x => x.CompanyID).FirstOrDefault();
            if (managerComp != 0)
            {
                List<Person> empList = _context.Employees.Where(x => x.CompanyID == managerComp && x.RoleEnum == ENTITIES.Enums.RoleEnum.Personel && x.ReportsTo == id).ToList();
             
                return View(empList);
            }
            return View(_person.GetByID(id));
        }


        [HttpGet]
        public IActionResult PersonelDeatails(int id)
        {
            Person getPerson = _person.GetByID(id);
            return View(getPerson);
        }

        [HttpGet]
        public IActionResult PersonelEdit(int id)
        {
            Person personUpdated = _person.GetByID(id);
            return View(personUpdated);
        }

        [HttpPost]
        public IActionResult PersonelEdit(Person person, int id)
        {
            Person employeeUpdated = _person.GetByID(id);
            employeeUpdated.Salary = person.Salary;
            employeeUpdated.FirstName = person.FirstName;
            employeeUpdated.LastName = person.LastName;
            employeeUpdated.MaidenName = person.MaidenName;
            employeeUpdated.MiddleName = person.MiddleName;
            employeeUpdated.DepartmansEnum = person.DepartmansEnum;
            employeeUpdated.Address = person.Address;
            employeeUpdated.Mail = person.Mail;
            employeeUpdated.PersonelPhoneNumber = person.PersonelPhoneNumber;
            employeeUpdated.HireDate = person.HireDate;
            if (employeeUpdated.HireDate>person.LeaveJobDate)
            {
                ViewBag.DateValid = "İşe Giriş Tarihinden Önce işten çıkartma yapılamaz";
                return View();
            }
            else
            {
                employeeUpdated.LeaveJobDate = person.LeaveJobDate;
            }
        
            employeeUpdated.Salary = Convert.ToDecimal(person.Salary);

            if (person.Photo != null)
            {
                employeeUpdated.PhotoName = ResimKaydet(person.Photo);
            }
            int idimmm = Convert.ToInt32(person.ReportsTo);
            if (DateTime.Now < person.LeaveJobDate || person.LeaveJobDate == new DateTime(0001, 01, 01))
            {
                person.IsActive = true;
            }
            _person.Update(employeeUpdated);
            return RedirectToAction("GetAllPersonel", new { id = idimmm });
        }
    }
}
