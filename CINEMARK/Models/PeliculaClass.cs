using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CINEMARK.Models
{
    public class PeliculaClass
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Director { get; set; }
        public string Genero { get; set; }
        public int Rating { get; set; }
        public string Imagen { get; set; }
        //public HttpPostedFileBase ImagenFile { get; set; }
    }
}