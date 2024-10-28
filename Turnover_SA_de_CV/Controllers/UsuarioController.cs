using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Turnover_SA_de_CV.Controllers
{

    public class UsuarioController : Controller
    {
        private readonly Turnover_databaseEntities1 _context;

        // Constructor sin parámetros
        public UsuarioController()
        {
            _context = new Turnover_databaseEntities1(); // Inicializar el contexto aquí
        }

        // Constructor que acepta un parámetro para inyección de dependencias
        public UsuarioController(Turnover_databaseEntities1 context)
        {
            _context = context;
        }

        // Acción para mostrar el formulario de registro
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Correo == usuario.Correo);
                if (usuarioExistente == null)
                {
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "El correo ya está registrado.");
                }
            }

            return View(usuario);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string correoElectronico, string contraseña)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correoElectronico && u.Contraseña == contraseña);
            if (usuario != null)
            {
                // Usar Session para almacenar datos
                Session["UsuarioId"] = usuario.Id.ToString();
                Session["NombreUsuario"] = usuario.Nombre;
                return RedirectToAction("Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Credenciales incorrectas.");
                return View();
            }
        }

        public ActionResult Dashboard()
        {
            var usuarioId = Session["UsuarioId"];
            if (usuarioId != null)
            {
                // Obtener el listado de conciertos disponibles
                var conciertos = _context.Conciertos.ToList();

                // Obtener el historial de compras del usuario autenticado, incluyendo la sección desde Entradas
                var compras = _context.Entradas
                    .Where(e => e.UsuarioId.ToString() == usuarioId.ToString())
                    .Select(e => new {
                        e.Concierto.Nombre,
                        e.FechaCompra,
                        e.Seccion,
                        e.Cantidad
                    })
                    .ToList();

                // Pasar ambos datos a la vista usando ViewBag
                ViewBag.ConciertosDisponibles = conciertos;
                ViewBag.HistorialCompras = compras;

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            // Limpiar la sesión
            Session.Clear();
            return RedirectToAction("Login");
        }


    }
}