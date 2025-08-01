using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProClinic.Models;
using ProClinic.Services;

namespace ProClinic.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly int pageSize = 5;

        public AppointmentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int pageIndex, string? search, string? column, string? orderBy)
        {
            IQueryable<Appointment> query = context.Appointments.Include(d => d.Doctor).Include(p => p.Patient);

            // search functionality
            if (search != null)
            {
                query = query.Where(p => p.PatientName.Contains(search) );
            }

            // sort functionality
            string[] validColumn = { "Id", "Doctor", "Symptoms", "DoctorId", "CreatedAt" };
            string[] validOrderBy = { "desc", "asc" };

            if (!validColumn.Contains(column))
            {
                column = "Id";
            }

            if (!validOrderBy.Contains(orderBy))
            {
                orderBy = "desc";
            }

            if (column == "Doctor")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.DoctorId);
                }
                else
                {
                    query = query.OrderByDescending(p => p.PatientName);
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

            var appointments = query.ToList();

            // passing pageIndex and total number of pages using ViewData dictionary
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
            // search
            ViewData["Search"] = search ?? "";
            // sort and orderBy
            ViewData["Column"] = column;
            ViewData["OrderBy"] = orderBy;

            return View(appointments);

            
        }

        public IActionResult Create()
        {
            ViewBag.DoctorList = new SelectList(context.Doctor, "Id", "Name");
            ViewBag.PatientList = new SelectList(context.Patients, "Id", "Name");

            return View();
        }


        [HttpPost]
        public IActionResult Create(AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DoctorList = new SelectList(context.Doctor, "Id", "Name");
                ViewBag.PatientList = new SelectList(context.Patients, "Id", "Name");

                return View();
            }

            if(appointmentDto.PatientName != null)
            {
                appointmentDto.PatientId = null;
            }

            Appointment appointMent = new Appointment()
            {
                DoctorId = appointmentDto.DoctorId,
                PatientId = appointmentDto.PatientId,
                AppointmentDateTime = appointmentDto.AppointmentDateTime,
                Reason = appointmentDto.Reason,
                Status = appointmentDto.Status,
                PatientName = appointmentDto.PatientName,
                Phone = appointmentDto.Phone,
                Address = appointmentDto.Address
            };

            context.Appointments.Add(appointMent);
            context.SaveChanges();

            return RedirectToAction("Index", "Appointments");
        }
    }
}
