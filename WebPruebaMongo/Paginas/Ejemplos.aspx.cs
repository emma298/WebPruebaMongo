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
	public partial class Ejemplos : System.Web.UI.Page
	{
        private readonly NotacionCientificaHelper.ConvertidorNotacionCientifica convertidor =
            new NotacionCientificaHelper.ConvertidorNotacionCientifica();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                resultContainer.Visible = false;
            }
        }

        protected void btnConvertir_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtNumero.Text, out double numero))
            {
                string notacionCientifica = convertidor.ConvertirANotacionCientifica(numero);

                // Mostrar resultados
                lblResultado.Text = notacionCientifica;

                // Mostrar detalles de la conversión
                int exponente = (int)Math.Floor(Math.Log10(Math.Abs(numero)));
                double coeficiente = numero / Math.Pow(10, exponente);
                lblDetalleConversion.Text = $@"
                    <strong>Descomposición:</strong> {numero} = {coeficiente} × 10<sup>{exponente}</sup><br>
                    <strong>Paso a paso:</strong> 
                    1) Encontramos el exponente (n) donde 10<sup>n</sup> es menor o igual al número.<br>
                    2) Dividimos el número original por 10<sup>{exponente}</sup> para obtener el coeficiente.";

                resultContainer.Visible = true;
            }
            else
            {
                lblResultado.Text = "❌ Por favor, ingresa un número válido.";
                lblDetalleConversion.Text = "Ejemplos válidos: 4500000, 0.000032, -12500, 3.14e5";
                resultContainer.Visible = true;
            }
        }
    }
}