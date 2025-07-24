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

        public DoctorsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            return View();
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

            return RedirectToAction("Index", "Doctor");
        }
    }
}
