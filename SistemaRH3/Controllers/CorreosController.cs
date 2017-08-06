using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using SistemaRH3.Models;

namespace SistemaRH3.Controllers
{
    [Authorize]
    public class CorreosController : Controller
    {

        SistemaRH3.DAL.GeneralContext db = new SistemaRH3.DAL.GeneralContext();
       
        


        // GET: Correos
        public ActionResult Index()
        {
            return View();
        }


        //correo multiple
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CorreoMultiple(string[] destinatariosArray, string asunto, string mensaje, string accion)
        {
            string correo = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Email).First().ToString();
            string pass = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Password).First().ToString();
           

            MailMessage mmsg = new MailMessage();

            if (accion == "Enviar") { 

            for (int i = 0; i < destinatariosArray.Length; i++)
            {
                var product = db.Empleados.ToList().Find(x => x.EmpleadoID == int.Parse(destinatariosArray[i]));
                mmsg.To.Add(product.email);

            }
                                    
            //Asunto
            mmsg.Subject = asunto;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            //Cuerpo del Mensaje
            mmsg.Body = mensaje;
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML

            //Correo electronico desde la que enviamos el mensaje
            mmsg.From = new MailAddress(correo);


            /*-------------------------CLIENTE DE CORREO----------------------*/

            //Creamos un objeto de cliente de correo
            SmtpClient cliente = new SmtpClient();

            //Hay que crear las credenciales del correo emisor
            cliente.Credentials =
        new System.Net.NetworkCredential(correo, pass);


            cliente.Port = 587;
            cliente.EnableSsl = true;


            cliente.Host = "smtp.gmail.com"; //Para Gmail "smtp.gmail.com";


            /*-------------------------ENVIO DE CORREO----------------------*/
            bool enviado = true;
            try
            {
                //Enviamos el mensaje      
                cliente.Send(mmsg);

            }
            catch (SmtpException ex)
            {
                enviado = false;
            }
            GuardarMultiples(asunto, destinatariosArray, mensaje, enviado);
        }else if(accion=="Archivar")

            {
                GuardarMultiples(asunto, destinatariosArray, mensaje, false);
             }   
            
            return Json(new { Estatus = "OK" });
        }

        //Guardar correo multiple
        public void GuardarMultiples(string asunto, string[]destinatariosArray, string mensaje,bool estado)
        {
            CorreosMultiples correo = new CorreosMultiples();

            string correos="";
            for(int i = 0; i < destinatariosArray.Length; i++)
            {
                var product = db.Empleados.ToList().Find(x => x.EmpleadoID == int.Parse(destinatariosArray[i]));

                correos = product.nombre + " " + product.apellido + ", " + correos;
            }

            correo.destinatarios = correos;
            correo.asunto = asunto;
            correo.mensaje = mensaje;
            correo.fecha = DateTime.Now;
            if (estado)
            {
                correo.estado = "Enviado";               
            }
            else
            {
                correo.estado = "Archivado";
            }

            db.CorreosMultiples.Add(correo);
            db.SaveChanges();
          
            
        }
        //correo simple
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CorreoSimple([Bind(Include = "CorreoID,asunto,mensaje,EmpleadoID")]
        Correos correos, string accion)
        {
            string correo = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Email).First().ToString();
            string pass = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Password).First().ToString();
           

            if (accion == "Enviar")
            {
                 var product = db.Empleados.ToList().Find(x => x.EmpleadoID == correos.EmpleadoID);

                string destinatario = product.email;
                string asunto = correos.asunto;
                string mensaje = correos.mensaje;

                /*-------------------------MENSAJE DE CORREO----------------------*/

                //Creando un nuevo Objeto de mensaje
                MailMessage mmsg = new MailMessage();

                //Direccion de correo electronico a la que queremos enviar el mensaje
                mmsg.To.Add(destinatario);

                //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario

                //Asunto
                mmsg.Subject = asunto;
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

                //Direccion de correo electronico que queremos que reciba una copia del mensaje
                //mmsg.Bcc.Add("destinatariocopia@servidordominio.com"); //Opcional

                //Cuerpo del Mensaje
                mmsg.Body = mensaje;
                mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML

                //Correo electronico desde la que enviamos el mensaje
                mmsg.From = new MailAddress(correo);


                /*-------------------------CLIENTE DE CORREO----------------------*/

                //Creamos un objeto de cliente de correo
                SmtpClient cliente = new SmtpClient();

                //Hay que crear las credenciales del correo emisor
                cliente.Credentials =
            new System.Net.NetworkCredential(correo, pass);


                cliente.Port = 587;
                cliente.EnableSsl = true;


                cliente.Host = "smtp.gmail.com"; //Para Gmail "smtp.gmail.com";


                /*-------------------------ENVIO DE CORREO----------------------*/
                bool enviado = true;
                try
                {
                    //Enviamos el mensaje      
                    cliente.Send(mmsg);
                    
                }
                catch (SmtpException ex)
                {
                    enviado = false;
                }
                Guardar(correos.asunto, correos.EmpleadoID, correos.mensaje, enviado);
            }else if(accion=="Archivar")
            {
                Guardar(correos.asunto, correos.EmpleadoID, correos.mensaje, false);
            }
            

            return View("Index",db.Correos);
        }

        //correo  no registrado
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CorreoNuevo([Bind(Include = "CorreoID,asunto,mensaje")]
        Correos correos, string destinatario, string accion)
        {
            string correo = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Email).First().ToString();
            string pass = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Password).First().ToString();
            

            string estado = "Enviado";

            if (accion == "Enviar")
            {
                string asunto = correos.asunto;
                string mensaje = correos.mensaje;

                /*-------------------------MENSAJE DE CORREO----------------------*/

                //Creando un nuevo Objeto de mensaje
                MailMessage mmsg = new MailMessage();

                //Direccion de correo electronico a la que queremos enviar el mensaje
                mmsg.To.Add(destinatario);

                //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario

                //Asunto
                mmsg.Subject = asunto;
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

                //Direccion de correo electronico que queremos que reciba una copia del mensaje
                //mmsg.Bcc.Add("destinatariocopia@servidordominio.com"); //Opcional

                //Cuerpo del Mensaje
                mmsg.Body = mensaje;
                mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML

                //Correo electronico desde la que enviamos el mensaje
                mmsg.From = new MailAddress(correo);


                /*-------------------------CLIENTE DE CORREO----------------------*/
                //Creamos un objeto de cliente de correo
                SmtpClient cliente = new SmtpClient();

                //Hay que crear las credenciales del correo emisor
                cliente.Credentials =
                new System.Net.NetworkCredential(correo, pass);


                cliente.Port = 587;
                cliente.EnableSsl = true;


                cliente.Host = "smtp.gmail.com"; //Para Gmail "smtp.gmail.com";

                /*-------------------------ENVIO DE CORREO----------------------*/
                
                try
                {
                    //Enviamos el mensaje      
                    cliente.Send(mmsg);

                }
                catch (SmtpException ex)
                {
                    estado = "Archivado";
                }
                
            }
            else if (accion == "Archivar")
            {
               estado = "Archivado";
            }


            GuardarNoRegistrado(correos.asunto, destinatario,correos.mensaje, estado);
            Historial("Correo enviado a ", destinatario, "");


            return View("Index", db.Correos);
        }

        //guardar correo no registrado
        public void GuardarNoRegistrado(string asunto, string destinatario, string mensaje, string estado)
        {

            CorreosNoRegistrados correo = new CorreosNoRegistrados();
            correo.destinatario = destinatario;
            correo.asunto = asunto;
            correo.mensaje = mensaje;
            correo.estado = estado;
            correo.fecha = DateTime.Now;
            

            db.CorreosNoRegistrados.Add(correo);
            db.SaveChanges();


        }

        //guargar el mensaje
        public void Guardar(string asunto, int EmpleadoID, string mensaje, bool enviado)
        {
            Correos correo = new Correos();
            correo.asunto = asunto;
            correo.mensaje = mensaje;
            correo.EmpleadoID = EmpleadoID;
            correo.fecha = DateTime.Now;
            

            if (enviado)
            {
                correo.estado = "Enviado";
            }
            else
            {
                correo.estado = "Archivado";
            }



            if (EmpleadoID != 0)
            {
                var product = db.Empleados.ToList().Find(x => x.EmpleadoID == EmpleadoID);

                db.Correos.Add(correo);
                db.SaveChanges();
                Historial("Se envio un correo a: ", product.nombre, product.apellido);
            }



        }
        
        //reenviar el mensaje
        [HttpPost]
        public ActionResult Reenviar(int id,string tipo)
        {
            string correoo = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Email).First().ToString();
            string pass = db.ConfigCorreo.OrderByDescending(p => p.ID).Select(r => r.Password).First().ToString();
            

            string destinatario = "";
            string asunto = "";
            string mensaje = "";

            if (tipo == "registrado")
            {
                var correo = db.Correos.ToList().Find(x => x.CorreoID == id);
                var product = db.Empleados.ToList().Find(x => x.EmpleadoID == correo.EmpleadoID);
                destinatario = product.email;
                asunto = correo.asunto;
                 mensaje = correo.mensaje;

            }else if(tipo=="no registrado")
            {
                var correo = db.CorreosNoRegistrados.ToList().Find(x => x.CorreoID == id);
               
                 destinatario = correo.destinatario;
                 asunto = correo.asunto;
                 mensaje = correo.mensaje;

            }
           
            

            /*-------------------------MENSAJE DE CORREO----------------------*/

            //Creando un nuevo Objeto de mensaje
            MailMessage mmsg = new MailMessage();

                //Direccion de correo electronico a la que queremos enviar el mensaje
                mmsg.To.Add(destinatario);

                //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario

                //Asunto
                mmsg.Subject = asunto;
                mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

                //Direccion de correo electronico que queremos que reciba una copia del mensaje
                //mmsg.Bcc.Add("destinatariocopia@servidordominio.com"); //Opcional

                //Cuerpo del Mensaje
                mmsg.Body = mensaje;
                mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML

                //Correo electronico desde la que enviamos el mensaje
                mmsg.From = new MailAddress(correoo);


                /*-------------------------CLIENTE DE CORREO----------------------*/
                //Creamos un objeto de cliente de correo
                SmtpClient cliente = new SmtpClient();

                //Hay que crear las credenciales del correo emisor
                cliente.Credentials =
                new System.Net.NetworkCredential(correoo, pass);


                cliente.Port = 587;
                cliente.EnableSsl = true;


                cliente.Host = "smtp.gmail.com"; //Para Gmail "smtp.gmail.com";

                /*-------------------------ENVIO DE CORREO----------------------*/
                bool enviado = true;
                try
                {
                    //Enviamos el mensaje      
                    cliente.Send(mmsg);

                }
                catch (SmtpException ex)
                {
                    enviado = false;
                }

           


            return Json(new{ Estatus= "OK"});
        }


        //Borrar correo
        [HttpPost]
        public ActionResult Borrar(int id,string tipo)
        {
            if (tipo == "registrado")
            {
                var product = db.Correos.ToList().Find(x => x.CorreoID == id);
                var empleado = db.Empleados.ToList().Find(x => x.EmpleadoID == product.EmpleadoID);
                if (product != null)
                {
                    db.Correos.Remove(product);
                    db.SaveChanges();
                }
                Historial("Se elimino el correo de ", empleado.nombre, empleado.apellido);

            }else if (tipo == "noregistrado")
            {
                var product = db.CorreosNoRegistrados.ToList().Find(x => x.CorreoID == id);
               // var empleado = db.Empleados.ToList().Find(x => x.EmpleadoID == product.EmpleadoID);
                if (product != null)
                {
                    db.CorreosNoRegistrados.Remove(product);
                    db.SaveChanges();
                }
                Historial("Se elimino el correo de ", product.destinatario,"");

            }
            else if (tipo == "multiple")
            {
                var product = db.CorreosMultiples.ToList().Find(x => x.CorreoID == id);
                // var empleado = db.Empleados.ToList().Find(x => x.EmpleadoID == product.EmpleadoID);
                if (product != null)
                {
                    db.CorreosMultiples.Remove(product);
                    db.SaveChanges();
                }
                //Historial("Se elimino el correo de ", empleado.nombre, empleado.apellido);

            }


            return Json(new { Estado="ok"});
        }

       



        //Registrar Historial
        public ActionResult Historial(string desc, string usuarioNombre, string usuarioApellido)
        {
            try
            {
                var result = db.Historial.Add(new Models.Historial
                {
                    descripcion = desc,
                    elementoNombre = usuarioNombre,
                    elementoApellido = usuarioApellido,
                    elemento = "correos",
                    fecha = DateTime.Now
                });
                db.SaveChanges();
                return Json(new
                {
                    Estatus = "OK",
                    Record = new
                    {
                        descripcion = result.descripcion,
                        elementoNombre = result.elementoNombre,
                        elementoApellido = result.elementoApellido,
                        elemento = result.elemento,
                        Fecha = result.fecha
                    }
                });
            }
            catch
            {
                return Json(new { Estatus = "ERROR" });
            }
        }


        //buscar empleados
        public ActionResult Buscar(List<Correos> correos)
        {
            return View(correos);
        }

        public List<Correos> BuscarRegistrado(string text)
        {
            var result = from c in db.Correos
                         where
                c.Empleados.nombre.Contains(text)
                         select c;

            return result.ToList();
        }

        public List<CorreosNoRegistrados> BuscarNoRegistrado(string text)
        {
            var result = from c in db.CorreosNoRegistrados
                         where
                c.destinatario.Contains(text)
                         select c;

            return result.ToList();
        }
        public List<CorreosMultiples> BuscarMultiple(string text)
        {
            var result = from c in db.CorreosMultiples
                         where
                c.destinatarios.Contains(text)
                         select c;

            return result.ToList();
        }

       
        //Buscar Correo 
        [HttpPost]
        public ActionResult BuscarCorreo(string filtro, string parametro)
        {
            

            if (filtro.ToLower() == "registrado")
            {
                var resultado = BuscarRegistrado(parametro);
                if (resultado.Count() != 0)
                {
                    return PartialView("~/Views/Correos/Registrados/ResultadoBuscar.cshtml", resultado);
                }
                else
                {
                    return PartialView("~/Views/Correos/Registrados/BusquedaNula.cshtml");
                }


            }
            else if (filtro.ToLower() == "noregistrado")
            {
                var resultado = BuscarNoRegistrado(parametro);
                if (resultado.Count() != 0)
                {
                    return PartialView("~/Views/Correos/NoRegistrados/ResultadoBuscar.cshtml", resultado);
                }
                else
                {
                    return PartialView("~/Views/Correos/NoRegistrados/BusquedaNula.cshtml");
                }
            }
            else if (filtro.ToLower() == "multiple")
            {
                var resultado = BuscarMultiple(parametro);
                if (resultado.Count() != 0)
                {
                    return PartialView("~/Views/Correos/Multiples/ResultadoBuscar.cshtml", resultado);
                }
                else
                {
                    return PartialView("~/Views/Correos/Multiples/BusquedaNula.cshtml");
                }
            }else
            {
                return View("~/Views/Correos/Buscar.cshtml");
            }
            
        }




    }

}