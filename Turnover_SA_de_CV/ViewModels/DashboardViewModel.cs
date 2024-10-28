using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Turnover_SA_de_CV;

namespace Turnover_SA_de_CV.ViewModels
{
    public class DashboardViewModel
    {
        public List<Concierto> ListaConciertos { get; set; }
        public List<HistorialCompraViewModel> HistorialCompras { get; set; }
    }

    public class HistorialCompraViewModel
    {
        public string NombreConcierto { get; set; }
        public string TipoEntrada { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCompra { get; set; }
        public string Lugar { get; set; }
        public decimal TotalPagado { get; set; }
    }

}