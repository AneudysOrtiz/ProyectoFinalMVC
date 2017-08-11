using SistemaRH3.DAL;
using SistemaRH3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaRH3.Controllers
{
    public class UsuariosController : Controller
    {
        GeneralContext db = new GeneralContext();
        // GET: Usuarios
        public ActionResult Index()
        {
            return View("MiCuenta",db.Usuarios.ToList());
        }

        [HttpPost]
        public ActionResult Editar(int id, string nombre, string apellido,string direccion, string telefono, HttpPostedFileBase foto)
        {
            
            var product = db.Usuarios.ToList().Find(x => x.correo == User.Identity.Name);

            if (product != null)
            {
                product.nombre = nombre;
                product.apellido = apellido;
                product.direccion = direccion;
                product.telefono = telefono;
                product.correo = User.Identity.Name;

                if (foto != null)
                {
                    if (foto.ContentLength > 0)
                    {
                        if (Path.GetExtension(foto.FileName).ToLower() == ".jpg"
                            || Path.GetExtension(foto.FileName).ToLower() == ".png"
                            || Path.GetExtension(foto.FileName).ToLower() == ".jpeg"
                            || Path.GetExtension(foto.FileName).ToLower() == ".gif")
                        {
                            var fileName = Path.GetFileName(foto.FileName); //getting only file name 
                            fileName = product.nombre.ToString() + product.apellido.ToString() + fileName;
                            var path = Path.Combine(Server.MapPath("~/uploads/pictures"), fileName);
                            product.foto = fileName;
                            //product.foto = path;
                            foto.SaveAs(path);

                        }
                    }
                }

                db.Entry(product).State = EntityState.Modified;

            }else
            {
                Usuarios usuario = new Usuarios();
                usuario.nombre = nombre;
                usuario.apellido = apellido;
                usuario.direccion = direccion;
                usuario.telefono = telefono;
                usuario.correo = User.Identity.Name;
                db.Usuarios.Add(usuario);

            }

           
            db.SaveChanges();

            return View("MiCuenta",db.Usuarios.ToList());
        }
    }
}