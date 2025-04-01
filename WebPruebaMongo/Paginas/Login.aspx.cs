using System;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using WebPruebaMongo.Clases;

namespace WebPruebaMongo.Paginas
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMensaje.Visible = false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var (success, message, usuario) = AuthHelper.AutenticarUsuario(
                txtUsuario.Text,
                txtPassword.Text,
                chkRecordar.Checked
            );

            if (success)
            {
                Session["Usuario"] = usuario.Nombre;
                Response.Redirect("Definicion.aspx", false);
            }
            else
            {
                MostrarMensaje(message, "danger");
            }
        }

        // Método auxiliar para mostrar mensajes
        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = $"alert alert-{tipo}";
            lblMensaje.Visible = true;
        }
    }
}