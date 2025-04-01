using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPruebaMongo.Clases;
using static WebPruebaMongo.Clases.NotacionCientificaHelper;

namespace WebPruebaMongo.Paginas
{
	public partial class Definicion : System.Web.UI.Page
	{
        private static readonly Random random = new Random();
        private readonly NotacionCientificaHelper.ConvertidorNotacionCientifica convertidor =
            new NotacionCientificaHelper.ConvertidorNotacionCientifica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerarEjemploAleatorio();
            }
        }

        private void GenerarEjemploAleatorio()
        {
            // Generar números con diferentes magnitudes
            int[] exponentes = { -15, -12, -9, -6, -3, -2, -1, 0, 1, 2, 3, 6, 9, 12, 15 };
            int exponente = exponentes[random.Next(exponentes.Length)];
            double coeficiente = Math.Round(random.NextDouble() * 9 + 1, 6);

            // 30% de probabilidad de ser negativo
            if (random.NextDouble() < 0.3)
            {
                coeficiente *= -1;
            }

            double numero = coeficiente * Math.Pow(10, exponente);
            string notacionCientifica = convertidor.ConvertirANotacionCientifica(numero);

            lblNumeroOriginal.Text = NotacionCientificaHelper.FormatearNumero(numero);
            lblNotacionCientifica.Text = notacionCientifica;
        }
    }
}