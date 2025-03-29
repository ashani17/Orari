using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;

namespace Orari.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStudentRepository _studentRepository;

        public StudentController(AppDbContext context, IStudentRepository studentRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
