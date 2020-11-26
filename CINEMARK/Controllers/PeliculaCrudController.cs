using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CINEMARK.Models;
using System.IO;
using System.Web;

namespace CINEMARK.Controllers
{
    public class PeliculaCrudController : ApiController
    {
        cinemarkEntities cartelera = new cinemarkEntities();

        //mostrar todas las peliculas desde la base de datos
        public IHttpActionResult GetPelicula()
        {
            var results = cartelera.peliculasTables.ToList();
            return Ok(results);
        }

        //insertar nueva pelicula
        [HttpPost]
        public IHttpActionResult PeliculaInsert(peliculasTable peliculainsert)
        {
            string foldercreate = HttpContext.Current.Server.MapPath("~/Images/");
            if(!Directory.Exists(foldercreate))
            {
                Directory.CreateDirectory(foldercreate);
            }
            if(HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var fileupload = HttpContext.Current.Request.Files["Imgpathsave"];
                if(fileupload!=null)
                {
                    var saveimages = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/"), fileupload.FileName);
                    fileupload.SaveAs(saveimages);
                    cartelera.peliculasTables.Add(new peliculasTable
                    {
                        titulo = peliculainsert.titulo,
                        sinopsis = peliculainsert.sinopsis,
                        director = peliculainsert.director,
                        genero = peliculainsert.genero,
                        rating = peliculainsert.rating,
                        imagen = fileupload.FileName
                    });
                    cartelera.SaveChanges();
                }
            }
            return Ok();
            
            /*cartelera.peliculasTables.Add(peliculainsert);
            cartelera.SaveChanges();
            return Ok();*/
        }

        public IHttpActionResult GetPeliculaId(int id)
        {
            PeliculaClass peliculadetails = null;
            peliculadetails = cartelera.peliculasTables.Where(x => x.id == id).Select(x => new PeliculaClass()
            {
                Id = x.id,
                Titulo = x.titulo,
                Sinopsis = x.sinopsis,
                Director = x.director,
                Genero = x.genero,
                Rating = x.rating,
                Imagen = x.imagen,
            }).FirstOrDefault<PeliculaClass>();

            if (peliculadetails == null)
            {
                return NotFound();
            }

            return Ok(peliculadetails);
        }

        public IHttpActionResult Put(PeliculaClass pc)
        {
            var updatepelicula = cartelera.peliculasTables.Where(x => x.id == pc.Id).FirstOrDefault<peliculasTable>();
            if (updatepelicula != null)
            {
                updatepelicula.id = pc.Id;
                updatepelicula.titulo = pc.Titulo;
                updatepelicula.sinopsis = pc.Sinopsis;
                updatepelicula.director = pc.Director;
                updatepelicula.genero = pc.Genero;
                updatepelicula.rating = pc.Rating;
                updatepelicula.imagen = pc.Imagen;
                cartelera.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        public IHttpActionResult SearchTitulo(string search)
        {
            var result = cartelera.peliculasTables.Where(x => x.titulo.StartsWith(search) || search == null).ToList();
            return Ok(result);
        }

        public IHttpActionResult Delete(int id)
        {
            var borrar = cartelera.peliculasTables.Where(x => x.id == id).FirstOrDefault();
            cartelera.Entry(borrar).State = System.Data.Entity.EntityState.Deleted;
            cartelera.SaveChanges();
            return Ok();
        }
    }
}
