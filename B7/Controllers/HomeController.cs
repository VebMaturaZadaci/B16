using B16.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;

namespace B16.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SingleFileUpload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (!IsFilenameValid(file.FileName))
                {
                    ModelState.AddModelError("file", "Invalid file name.");
                    return View("Index");
                }

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return RedirectToAction(nameof(Index));
        }


        //proverava da li je ime null ili empty,
        //proverava da li sadrzi nedozvoljene karaktere,
        //proverava da li ima extenziju
        private bool IsFilenameValid(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            string[] invalidChars = { "\"", "<", ">", "|", "\0", "\\", "/", ":", "*", "?" };
            if (invalidChars.Any(fileName.Contains))
            {
                return false;
            }

            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }

            return true;
        }
    }
}
