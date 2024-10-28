using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Turnover_SA_de_CV.ViewModels;

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
                var viewModel = new DashboardViewModel
                {
                    ListaConciertos = _context.Conciertos.ToList(), // Cambiado de 'db' a '_context'
                    HistorialCompras = _context.Entradas.Select(e => new HistorialCompraViewModel
                    {
                        NombreConcierto = e.Concierto.Nombre,
                        TipoEntrada = e.Seccion,
                        Cantidad = e.Cantidad,
                        FechaCompra = e.FechaCompra,
                        Lugar = e.Concierto.Lugar, // Asegúrate de que el modelo Concierto tenga esta propiedad
                        TotalPagado = e.TotalPagado
                    }).ToList()
                };

                return View(viewModel);
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