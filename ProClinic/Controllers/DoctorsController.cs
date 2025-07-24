using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProClinic.Models;
using ProClinic.Services;

namespace ProClinic.Controllers
{
    [Authorize(Roles ="admin")]
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly int pageSize = 5;

        public DoctorsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

     
        public IActionResult Index(int pageIndex, string? search, string? column, string? orderBy)
        {
            IQueryable<Doctor> query = context.Doctor;

            // search functionality
            if (search != null)
            {
                query = query.Where(p => p.Name.Contains(search) || p.Specialization.Contains(search));
            }

            // sort functionality
            string[] validColumn = { "Id", "Name", "Specialization", "CreatedAt" };
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
            else if (column == "Specialization")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Specialization);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Specialization);
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
            return View();
        }

        [HttpPost]
        public IActionResult Create(DoctorDto doctorDto)
        {
            Console.WriteLine(doctorDto);

            if (doctorDto.ImageFileName == null)
            {
                ModelState.AddModelError("ImageFileName", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            // uploading the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(doctorDto.ImageFileName!.FileName);

            string imageFullPath = environment.WebRootPath + "/doctors/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                doctorDto.ImageFileName.CopyTo(stream);
            }

            Doctor doctor = new Doctor()
            {
                Name = doctorDto.Name,
                DateOfBirth = doctorDto.DateOfBirth,
                Email = doctorDto.Email,
                Gender = doctorDto.Gender,
                Specialization = doctorDto.Specialization,
                phone = doctorDto.phone,
                Details = doctorDto.Details,
                Address = doctorDto.Address,
                ImageFileName = newFileName

            };

            context.Doctor.Add(doctor);
            context.SaveChanges();

            return RedirectToAction("Index", "Doctors");
        }
    }
}
