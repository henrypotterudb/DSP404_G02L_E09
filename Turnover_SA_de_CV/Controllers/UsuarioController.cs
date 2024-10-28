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
            _context = new Turnover_databaseEntities1(); // Inicializar el contexto
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
                    usuario.Rol = "Cliente"; // Asigna el rol "Cliente" por defecto
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
                Session["UsuarioId"] = usuario.Id.ToString();
                Session["NombreUsuario"] = usuario.Nombre;
                Session["Rol"] = usuario.Rol; 

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
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login");
            }

            // Solo los clientes pueden acceder a este Dashboard
            if (Session["Rol"].ToString() != "Cliente")
            {
                return RedirectToAction("AdminDashboard"); // Redirigir a un dashboard de administrador
            }

            int usuarioId = int.Parse(Session["UsuarioId"].ToString());

            var viewModel = new DashboardViewModel
            {
                ListaConciertos = _context.Conciertos.ToList(),
                HistorialCompras = _context.Entradas
                    .Where(e => e.UsuarioId == usuarioId)
                    .Select(e => new HistorialCompraViewModel
                    {
                        NombreConcierto = e.Concierto.Nombre,
                        TipoEntrada = e.Seccion,
                        Cantidad = e.Cantidad,
                        FechaCompra = e.FechaCompra,
                        Lugar = e.Concierto.Lugar,
                        TotalPagado = e.TotalPagado
                    }).ToList()
            };

            return View(viewModel);
        }

        public ActionResult AdminDashboard()
        {
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login");
            }

            // Solo los administradores pueden acceder a este Dashboard
            if (Session["Rol"].ToString() != "Administrador")
            {
                return RedirectToAction("Dashboard"); // Redirigir al dashboard de clientes si no es admin
            }

            var viewModel = new AdminDashboardViewModel
            {
                ListaConciertos = _context.Conciertos.ToList(),
                ListaEntradas = _context.Entradas.ToList()
            };

            return View(viewModel);
        }



        public ActionResult Logout()
        {
            // Limpiar la sesión
            Session.Clear();
            return RedirectToAction("Login");
        }


    }
}