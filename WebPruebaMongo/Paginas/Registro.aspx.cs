using System;
using System.Web.UI;
using WebPruebaMongo.Clases;

namespace WebPruebaMongo.Paginas
{
    public partial class Registro : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMensaje.Visible = false;
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            var (success, message) = AuthHelper.RegistrarUsuario(
                txtUsuario.Text,
                txtPassword.Text,
                txtConfirmPassword.Text
            );

            if (success)
            {
                // Autenticar al usuario directamente después del registro
                var (authSuccess, authMessage, usuario) = AuthHelper.AutenticarUsuario(
                    txtUsuario.Text,
                    txtPassword.Text
                );

                if (authSuccess)
                {
                    Session["Usuario"] = usuario.Nombre;
                    Response.Redirect("Definicion.aspx", false);
                    return;
                }
            }

            MostrarMensaje(message, success ? "success" : "danger");
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = $"alert alert-{tipo}";
            lblMensaje.Visible = true;
        }
    }
}