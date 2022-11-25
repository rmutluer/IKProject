using Human_Resources_Management.ENTITIES.Entities;
using Human_Resources_Management.ENTITIES.ViewModel;
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
using System.Threading.Tasks;

namespace Human_Resources_Management.UI.Controllers
{
    [Authorize(Roles = "Personel")]
    public class EmployeeController : Controller
    {
        private readonly IGenericService<Person> _person;
        private readonly IWebHostEnvironment _env;
        private readonly HRMDbContext _context;
        private readonly IGenericService<Company> _company;
        private readonly IGenericService<Expenses> _expenses;
        private readonly IGenericService<AdvancePayment> _advance;
        private readonly IGenericService<Vacation> _vacation;

        public EmployeeController(IGenericService<Person> person, IWebHostEnvironment env, HRMDbContext context, IGenericService<Company> company, IGenericService<Expenses> expenses, IGenericService<AdvancePayment> advance, IGenericService<Vacation> vacation)
        {
            _person = person;
            _env = env;
            _context = context;
            _company = company;
            _expenses = expenses;
            _advance = advance;
            _vacation = vacation;
        }

        public IActionResult Index()
        {
            List<Person> person = _context.Employees.Where(x => x.RoleEnum == ENTITIES.Enums.RoleEnum.Personel).ToList();
            return View(person);
        }


        [Route("Employee/GetPersonel/{id}")]
        public IActionResult GetPersonel(int id)
        {
            Person userEntrance = _context.Employees.Where(x => x.ID == id).FirstOrDefault();
            return View(_person.GetByID(id));
        }

        [HttpGet]
        public IActionResult UpdatePersonel(int id)
        {
            return View(_person.GetByID((int)id));
        }

        [HttpPost]
        public IActionResult UpdatePersonel([FromForm] Person person, int id)
        {
            Person managerUpdated = _person.GetByID(id);
            managerUpdated.Address = person.Address;
            managerUpdated.PersonelPhoneNumber = person.PersonelPhoneNumber;
            if (person.Photo != null)
            {
                managerUpdated.PhotoName = ResimKaydet(person.Photo);
            }

            _person.Update(managerUpdated);
            return RedirectToAction(nameof(GetPersonel), new { id = id });
        }

        public IActionResult Details(int id)
        {
            return View(_person.GetByID(id));
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
                // burada da dosya yolunun tammaı yerine \img\ kısmını substirng olarak alsın ve return etsin istiyorum. asdşll
                //NOT: 2 adet \ işaretinden biri kaçış karakteri. Ama renkleri düzgün göremiyorum :)<3 gayet iyi duruyoD:
            }
        }
        public string ResimKaydet2(IFormFile img)
        {
            if (img.ContentType.Contains("jpeg") || img.ContentType.Contains("jpg") || img.ContentType.Contains("png") || img.ContentType.Contains("pdf"))
            {
                string uniqueName = img.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", uniqueName);

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    img.CopyTo(fs);
                }
                return uniqueName;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        //[Route("Employee/SpendingList/{iid}")]
        public IActionResult SpendingList(int id)
        {
            SpendingPerson spendingPerson = new SpendingPerson();
            spendingPerson.Expenses = _context.Expenses.Where(x => x.PersonID == id).ToList();
            return View(spendingPerson);
        }


        [HttpGet]
        public IActionResult CreateSpending(int id)
        {
            SpendPerson sp1 = new SpendPerson();
            sp1.Expenses = new Expenses();
            return View(sp1);
        }

        [HttpPost]
        public IActionResult CreateSpending(SpendPerson sp, int id)
        {
            var findID = _person.GetDefault(x => x.ID == id).FirstOrDefault();
            if (findID.LeaveJobDate > DateTime.Now || findID.LeaveJobDate == null)
            {
                if (ModelState.IsValid)
                {
                    var expenses = sp.Expenses;
                    sp.Person = _person.GetByID(id);
                    expenses.PersonID = sp.Person.ID;
                    expenses.SpendingRequestDate = DateTime.Now.Date;
                    expenses.SpendingResponseDate = null;
                    if (sp.Expenses.Photo != null)
                    {
                        expenses.PhotoName = ResimKaydet2(sp.Expenses.Photo);

                        if (String.IsNullOrEmpty(expenses.PhotoName))
                        {
                            ViewBag.Hata = "Yüklemek istediğiniz dosya sadece resim veya pdf formatı olmalıdır !";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Hata = "Fotoğraf Seçiniz";
                        return View();
                    }
                    //expenses.PhotoName = ResimKaydet2(sp.Expenses.Photo);
                    _expenses.Add(expenses);
                    return RedirectToAction("SpendingList", new { id });
                }

            }
            else
            {
                ViewBag.Stu = "Aktif Olarak Çalışmıyorsunuz";
                return View();
            }

            return View();
        }

        [HttpGet]
        public IActionResult UpdateSpending(int id)
        {
            _context.Expenses.Where(x => x.ID == id);
            return View(_expenses.GetByID(id));
        }

        [HttpPost]
        public IActionResult UpdateSpending(Expenses expenses, int id)
        {
            int iid = expenses.PersonID;
            Expenses ex = _expenses.GetByID(id);

            if (expenses.AproveStatus == ENTITIES.Enums.AproveStatus.OnayBekliyor)
            {
                ex.PersonID = expenses.PersonID;
                ex.Spending = expenses.Spending;
                ex.SpendingType = expenses.SpendingType;
                ex.PriceUnit = expenses.PriceUnit;
                if (expenses.Photo != null)
                {
                    ex.PhotoName = ResimKaydet2(expenses.Photo);
                }
                else
                {
                    ViewBag.Hata = "fotoğraf Seçiniz";
                }

            }
            _expenses.Update(ex);
            return RedirectToAction("SpendingList", new { id = iid });
        }
        [HttpGet]
        public IActionResult CreateAdvancePayment(int id)
        {
            PaymentPerson paymentPerson = new PaymentPerson();
            paymentPerson.AdvancePayment = new AdvancePayment();
            return View(paymentPerson);
        }

        [HttpPost]
        public IActionResult CreateAdvancePayment(PaymentPerson pp, int id)
        {
            var findID = _person.GetDefault(x => x.ID == id).FirstOrDefault();
            if (findID.LeaveJobDate > DateTime.Now || findID.LeaveJobDate == null)
            {
                if (ModelState.IsValid)
                {
                    decimal sum = 0;
                    var avanses = _context.AdvancePayments.Where(x => x.PersonID == id && (x.AproveStatus == ENTITIES.Enums.AproveStatus.OnayBekliyor || x.AproveStatus == ENTITIES.Enums.AproveStatus.Onaylandi) && x.AvansRequestDate.Date > DateTime.Now.AddDays(-365)).ToList();
                    foreach (var item in avanses)
                    {
                        if (item.PriceUnit == ENTITIES.Enums.PriceUnit.TL)
                        {
                            sum += item.Avans;
                        }
                        else if (item.PriceUnit == ENTITIES.Enums.PriceUnit.USD)
                        {
                            sum += item.Avans * 18.5m;
                        }
                        else if (item.PriceUnit == ENTITIES.Enums.PriceUnit.EUR)
                        {
                            sum += item.Avans * 19.25m;
                        }

                    }
                    var advancePayment = pp.AdvancePayment;
                    pp.Person = _person.GetByID(id);
                    advancePayment.PersonID = pp.Person.ID;
                    advancePayment.AvansRequestDate = DateTime.Now.Date;

                    if (sum < (pp.Person.Salary * 2))
                    {
                        _advance.Add(advancePayment);
                        return RedirectToAction("PaymentList", new { id });
                    }
                    else
                    {
                        ViewBag.AvansHata = "Artık Avans Talep Edemezsin";
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.Stu = "Aktif Olarak Çalışmıyorsunuz";
                return View();
            }
            return View();

        }

        [HttpGet]
        public IActionResult PaymentList(int id)
        {
            PersonPayments personPayment = new PersonPayments();
            personPayment.AdvancePayments = _context.AdvancePayments.Where(x => x.PersonID == id).ToList();
            return View(personPayment);
        }

        [HttpGet]
        public IActionResult UpdatePayment(int id)
        {
            _context.AdvancePayments.Where(x => x.ID == id);
            return View(_advance.GetByID(id));
        }

        [HttpPost]
        public IActionResult UpdatePayment(AdvancePayment advancePayment, int id)
        {
            int iid = advancePayment.PersonID;
            AdvancePayment ap = _advance.GetByID(id);

            if (ModelState.IsValid)
            {
                if (advancePayment.AproveStatus == ENTITIES.Enums.AproveStatus.OnayBekliyor)
                {
                    ap.PersonID = advancePayment.PersonID;
                    ap.Avans = advancePayment.Avans;
                    ap.Description = advancePayment.Description;
                    ap.PriceUnit = advancePayment.PriceUnit;

                }
                _advance.Update(ap);
                return RedirectToAction("PaymentList", new { id = iid });
            }
            return View();
        }


        [HttpGet]
        public IActionResult CreateVacation(int id)
        {
            PersonVacation pv = new PersonVacation();
            pv.Vacation = new Vacation();
            pv.Person = _person.GetByID(id);
            return View(pv);
        }


        [HttpPost]
        public IActionResult CreateVacation(PersonVacation pv, int id)
        {
            var findID = _person.GetDefault(x => x.ID == id).FirstOrDefault();
            if (findID.LeaveJobDate > DateTime.Now || findID.LeaveJobDate == null)
            {
               
                if (ModelState.IsValid)
                {
                    var vacation = pv.Vacation;
                    //vacation.PersonID = pv.Person.ID;
                    vacation.Person = _person.GetByID(id);
                    vacation.VacationRequestDate = DateTime.Now.Date;
                    vacation.Person.Gender = _context.Employees.Where(x => x.ID == id).Select(x => x.Gender).FirstOrDefault();
                    if (vacation.VacationStartDate > vacation.VacationRequestDate && vacation.VacationStartDate<vacation.VacationRequestDate.AddDays(365))
                    {
                        try
                        {
                            switch (vacation.VacationType)
                            {
                                case ENTITIES.Enums.VacationType.Yillik:
                                    if (vacation.VacationEndDate > vacation.VacationStartDate)
                                    {
                                        if (vacation.Person.HireDate > DateTime.Now.AddDays(-365))
                                        {
                                            ViewBag.YillikIzin = "1 Yılınızı doldurmadığınız için yıllık izin talep edemezsiniz.";
                                            return View();
                                        }
                                        else if (vacation.Person.HireDate < DateTime.Now.AddDays((5 * (-365))))
                                        {
                                            TimeSpan sum = DateTime.Now - DateTime.Now;
                                            vacation.VacationStartDate = pv.Vacation.VacationStartDate;
                                            vacation.VacationEndDate = pv.Vacation.VacationEndDate;
                                            TimeSpan ts = pv.Vacation.VacationEndDate - pv.Vacation.VacationStartDate;
                                            var sumVacation = _context.Vacations.Where(x => x.PersonID == id && x.AproveStatus == ENTITIES.Enums.AproveStatus.Onaylandi && x.VacationEndDate.Date > DateTime.Now.AddDays(-365)).ToList();
                                            foreach (var item in sumVacation)
                                            {
                                                sum += (item.VacationEndDate - item.VacationStartDate);
                                            }
                                            if ((ts + sum) > (DateTime.Now - DateTime.Now.AddDays(-30)))
                                            {
                                                ViewBag.Yillik = "Yıllık İzin Hakkınızı Doldurdunuz.";
                                                ViewBag.KalanHak = (DateTime.Now - DateTime.Now.AddDays(-30) - sum).Days;
                                                return View();
                                            }

                                        }
                                        else if (vacation.Person.HireDate > DateTime.Now.AddDays((5 * (-365))) && vacation.Person.HireDate < DateTime.Now.AddDays(-365))
                                        {
                                            TimeSpan sum = DateTime.Now - DateTime.Now;
                                            vacation.VacationStartDate = pv.Vacation.VacationStartDate;
                                            vacation.VacationEndDate = pv.Vacation.VacationEndDate;
                                            TimeSpan ts = pv.Vacation.VacationEndDate - pv.Vacation.VacationStartDate;
                                            var sumVacation = _context.Vacations.Where(x => x.PersonID == id && x.AproveStatus == ENTITIES.Enums.AproveStatus.Onaylandi && x.VacationEndDate.Date > DateTime.Now.AddDays(-365)).ToList();
                                            foreach (var item in sumVacation)
                                            {
                                                sum += (item.VacationEndDate - item.VacationStartDate);
                                            }
                                            if ((ts + sum) > (DateTime.Now - DateTime.Now.AddDays(-15)))
                                            {
                                                ViewBag.Yillik = "Yıllık İzin Hakkınızı Doldurdunuz.";
                                                ViewBag.KalanHak = (DateTime.Now - DateTime.Now.AddDays(-15) - sum).Days;
                                                return View();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.TarihiHataYillik = "Lütfen izin bitiş tarihini başlangıç tarihinden sonra bir tarih seçiniz.";
                                    }
                                    break;
                                case ENTITIES.Enums.VacationType.Dogum:
                                    if (vacation.Person.Gender == ENTITIES.Enums.GenderEnum.Kadın)
                                    {
                                        vacation.VacationStartDate = pv.Vacation.VacationStartDate;
                                        vacation.VacationEndDate = pv.Vacation.VacationStartDate.AddDays(180);

                                    }
                                    else
                                    {
                                        ViewBag.HataGenderE = "Erkekler Doğum İzni Alamaz!!!";
                                        return View();

                                    }

                                    break;
                                case ENTITIES.Enums.VacationType.Babalik:
                                    if (vacation.Person.Gender == ENTITIES.Enums.GenderEnum.Erkek)
                                    {
                                        vacation.VacationStartDate = pv.Vacation.VacationStartDate;
                                        vacation.VacationEndDate = pv.Vacation.VacationStartDate.AddDays(7);

                                    }
                                    else
                                    {
                                        ViewBag.HataGenderK = "Kadınlar Babalık İzni Alamaz!!!";
                                        return View();

                                    }

                                    break;
                                case ENTITIES.Enums.VacationType.Evlilik:
                                    vacation.VacationStartDate = pv.Vacation.VacationStartDate;
                                    vacation.VacationEndDate = pv.Vacation.VacationStartDate.AddDays(3);
                                    break;
                            }
                            _vacation.Add(vacation);
                            return RedirectToAction("VacationList", new { id });
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }
                    else
                    {
                        ViewBag.TarihiHata = "Lütfen gelecek bir tarih ve en fazla 1 yıl sonrası için tarihi seçiniz.";
                        return View();
                    }
                }
               
            }
            else
            {
                ViewBag.Stu = "Aktif Olarak Çalışmıyorsunuz";
                return View();
            }
            return RedirectToAction("VacationList", new { id });
        }

        [HttpGet]
        public IActionResult VacationList(int id)
        {
            PersonVacations pvs = new PersonVacations();
            pvs.Vacations = _context.Vacations.Where(x => x.PersonID == id).ToList();
            return View(pvs);
        }

        [HttpGet]
        public IActionResult RemoveVacation(int id)
        {
            var silinecekVacation = _vacation.GetByID(id);
            int iid = silinecekVacation.PersonID;
            _vacation.Remove(silinecekVacation);
            return RedirectToAction("VacationList", new { id = iid });
        }
    }
}
