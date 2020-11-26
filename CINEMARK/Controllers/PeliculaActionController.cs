using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CINEMARK.Models;
using System.Net.Http;
using System.IO;

namespace CINEMARK.Controllers
{
    public class PeliculaActionController : Controller
    {
        // GET: PeliculaAction
        public ActionResult Index(string search)
        {
            IEnumerable<peliculasTable> peliculaObject = null;
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44360/api/PeliculaCrud");

            var consumeapi = hc.GetAsync("PeliculaCrud?search=" + search);
            consumeapi.Wait();

            var readdata = consumeapi.Result;
            if (readdata.IsSuccessStatusCode)
            {
                var displaydata = readdata.Content.ReadAsAsync<IList<peliculasTable>>();
                displaydata.Wait();

                peliculaObject = displaydata.Result;
            }
            return View(peliculaObject);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(peliculasTable insertpelicula)
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44360/api/PeliculaCrud");
            /*insertar imagen
            string filename = Path.GetFileNameWithoutExtension(imagenFile.ImagenFile.FileName);
            string extension = Path.GetExtension(imagenFile.ImagenFile.FileName);
            imagenFile.Imagen = "../Image/" + filename;
            filename = Path.Combine(Server.MapPath("../Image/"), filename);
            imagenFile.ImagenFile.SaveAs(filename);*/

            var insertrecord = hc.PostAsJsonAsync<peliculasTable>("PeliculaCrud", insertpelicula);
            insertrecord.Wait();

            var savedata = insertrecord.Result;
            if (savedata.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        public ActionResult Details(int id)
        {
            PeliculaClass peliculaobject = null;
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44360/api/");

            var consumeapi = hc.GetAsync("PeliculaCrud?id=" + id.ToString());
            consumeapi.Wait();

            var readdata = consumeapi.Result;
            if (readdata.IsSuccessStatusCode)
            {
                var displaydata = readdata.Content.ReadAsAsync<PeliculaClass>();
                displaydata.Wait();
                peliculaobject = displaydata.Result;
            }

            return View(peliculaobject);
        }

        public ActionResult Edit(int id)
        {

            PeliculaClass peliculaobject = null;
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44360/api/");

            var consumeapi = hc.GetAsync("PeliculaCrud>id=" + id.ToString());
            consumeapi.Wait();

            var readdata = consumeapi.Result;
            if (readdata.IsSuccessStatusCode)
            {
                var displaydata = readdata.Content.ReadAsAsync<PeliculaClass>();
                displaydata.Wait();
                peliculaobject = displaydata.Result;
            }

            return View(peliculaobject);
        }

        [HttpPost]
        public ActionResult Edit(PeliculaClass pc)
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44360/api/PeliculaCrud");

            var insertrecord = hc.PutAsJsonAsync<PeliculaClass>("PeliculaCrud", pc);
            insertrecord.Wait();

            var savedata = insertrecord.Result;
            if (savedata.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "La pelicula no se pudo actualizar";
            }
            return View(pc);
        }

        public ActionResult Delete(int id)
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44360/api/PeliculaCrud");

            var delrecord = hc.DeleteAsync("PeliculaCrud/" + id.ToString());
            delrecord.Wait();

            var displaydata = delrecord.Result;
            if (displaydata.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}