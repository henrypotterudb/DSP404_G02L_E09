using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turnover_SA_de_CV.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Concierto> ListaConciertos { get; set; }
        public List<Entrada> ListaEntradas { get; set; }
    }
}