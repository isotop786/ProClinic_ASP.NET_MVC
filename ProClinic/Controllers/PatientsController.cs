using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProClinic.Services;
using ProClinic.Models;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProClinic.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly int pageSize = 5;

        public PatientsController(ApplicationDbContext context)
        {
            this.context = context;
        }


        public IActionResult Index(int pageIndex, string? search, string? column, string? orderBy)
        {
            IQueryable<Patient> query = context.Patients.Include(p=>p.Doctor);

            // search functionality
            if (search != null)
            {
                query = query.Where(p => p.Name.Contains(search) || p.Symptoms.Contains(search));
            }

            // sort functionality
            string[] validColumn = { "Id", "Name", "Symptoms", "DoctorId", "CreatedAt" };
            string[] validOrderBy = { "desc", "asc" };

            if (!validColumn.Contains(column))
            {
                column = "Id";
            }

            if (!validOrderBy.Contains(orderBy))
            {
                orderBy = "desc";
            }

            if (column == "Name")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Name);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Name);
                }
            }
            else if (column == "Symptoms")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Symptoms);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Symptoms);
                }
            }

            else if (column == "DoctorId")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.DoctorId);
                }
                else
                {
                    query = query.OrderByDescending(p => p.DoctorId);
                }
            }

            else if (column == "CreatedAt")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(p => p.CreatedAt);
                }
            }
            else
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Id);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Id);
                }

            }

            //query = query.OrderByDescending(p => p.Id);


            /// Pagination
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            // total number of products
            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            // Paginaiton end

            var products = query.ToList();

            // passing pageIndex and total number of pages using ViewData dictionary
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
            // search
            ViewData["Search"] = search ?? "";
            // sort and orderBy
            ViewData["Column"] = column;
            ViewData["OrderBy"] = orderBy;

            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.DoctorList = new SelectList(context.Doctor, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(PatientDto patientDto)
        {
            Console.WriteLine(patientDto);

            if (!ModelState.IsValid)
            {
                return View();
            }

            Patient patient = new Patient()
            {
                Name = patientDto.Name,
                DateOfBirth = patientDto.DateOfBirth,
                Phone = patientDto.Phone,
                Email = patientDto.Email,
                Address = patientDto.Address,
                Gender = patientDto.Gender,
                Symptoms = patientDto.Symptoms,
                paymentStatus = patientDto.paymentStatus,
                DoctorId = patientDto.DoctorId
            };
           

           

            context.Patients.Add(patient);
            context.SaveChanges();

            return RedirectToAction("Index", "Patients");
        }

        public async Task <IActionResult> Details(int Id)
        {
            var patient = await context.Patients.Include(p => p.Doctor).FirstOrDefaultAsync(p => p.Id == Id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        [HttpPost]
        public IActionResult UpdatePayment(UpdatePaymentDto dto)
        {
            Console.WriteLine("id " + dto.Id, "\nStatus: " + dto.PaymentStatus);

            var patient = context.Patients.FirstOrDefault(p => p.Id == dto.Id);

            if (patient != null)
            {
                patient.paymentStatus = dto.PaymentStatus;
                context.SaveChanges();
            }

            return RedirectToAction("Details", "Patients", new { id = dto.Id });
        }



    }
}
