using B16.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;

namespace B16.Controllers
{
    public class RasporedController : Controller
    {
        private readonly ILogger<RasporedController> _logger;
        private IWebHostEnvironment Environment;

        public RasporedController(ILogger<RasporedController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            Environment = environment;
        }

        public IActionResult Index()
        {
            List<Raspored> rasporedi = new List<Raspored>();
            XmlDocument doc = new XmlDocument();

            doc.Load(string.Concat(this.Environment.WebRootPath, "/uploads/Raspored.xml"));

            foreach (XmlNode node in doc.SelectNodes("/Rasporedi/Raspored"))
            {
                rasporedi.Add(new Raspored
                {
                    RedniBroj = int.Parse(node["Rbr"].InnerText),
                    Dan = node["DanUNedelji"].InnerText,
                    Predmet = node["Predmet"].InnerText
                });
            }

            return View(rasporedi.OrderBy(x => x.RedniBroj)
                                    .ThenBy(x => x.Dan)
                                    .ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
