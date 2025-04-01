using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using MongoDB.Driver;
using WebPruebaMongo.Clases;

namespace WebPruebaMongo.Paginas
{
    public partial class Evaluacion : System.Web.UI.Page
    {
        private readonly NotacionCientificaHelper helper = new NotacionCientificaHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["InicioEvaluacion"] == null)
                {
                    Session["InicioEvaluacion"] = DateTime.UtcNow.ToString();
                }

                GenerarEjercicios();
                feedbackSection.Visible = false;
                historialSection.Visible = false;
            }
        }

        private void GenerarEjercicios()
        {
            try
            {
                var ejercicios = new List<EjercicioViewModel>();

                for (int i = 1; i <= 3; i++)
                {
                    helper.GenerarEjercicio();
                    ejercicios.Add(new EjercicioViewModel
                    {
                        Numero = i,
                        Enunciado = helper.Ejercicio,
                        RespuestaCorrecta = helper.RespuestaCorrecta
                    });
                }

                rptEjercicios.DataSource = ejercicios;
                rptEjercicios.DataBind();
            }
            catch (Exception ex)
            {
                lblResultado.Text = $"<div class='alert alert-danger'>Error al generar ejercicios: {ex.Message}</div>";
                feedbackSection.Visible = true;
            }
        }

        protected void btnEvaluar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = Session["Usuario"]?.ToString() ?? "Invitado";
                int puntaje = 0;
                var feedbackItems = new List<FeedbackItem>();
                var respuestas = new List<BsonDocument>();
                var ejercicios = new List<BsonDocument>();

                // Validar que el usuario no sea nulo
                if (string.IsNullOrEmpty(usuario))
                {
                    throw new ArgumentException("El nombre de usuario no puede estar vacío");
                }

                foreach (RepeaterItem item in rptEjercicios.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        int numeroPregunta = item.ItemIndex + 1;
                        var txtRespuesta = (TextBox)item.FindControl("txtRespuesta");
                        var hfRespuestaCorrecta = (HiddenField)item.FindControl("hfRespuestaCorrecta");

                        // Validar controles
                        if (txtRespuesta == null || hfRespuestaCorrecta == null)
                        {
                            throw new NullReferenceException($"No se encontraron los controles para el ejercicio {numeroPregunta}");
                        }

                        string respuestaUsuario = txtRespuesta.Text?.Trim() ?? string.Empty;
                        string respuestaCorrecta = hfRespuestaCorrecta.Value?.Trim();

                        if (string.IsNullOrEmpty(respuestaCorrecta))
                        {
                            throw new ArgumentNullException($"La respuesta correcta para el ejercicio {numeroPregunta} es nula");
                        }

                        bool esCorrecta = helper.ValidarRespuesta(respuestaUsuario, respuestaCorrecta);
                        if (esCorrecta) puntaje++;

                        // Preparar feedback
                        feedbackItems.Add(new FeedbackItem
                        {
                            Pregunta = $"Ejercicio {numeroPregunta}",
                            Mensaje = esCorrecta
                                ? "✅ Correcto!"
                                : $"❌ Incorrecto. Tu respuesta: {respuestaUsuario}. Correcto: {respuestaCorrecta}",
                            ClaseCSS = esCorrecta ? "alert-success" : "alert-danger"
                        });

                        // Preparar datos para MongoDB con validación
                        var respuestaDoc = new BsonDocument
                        {
                            { "pregunta", numeroPregunta },
                            { "respuestaUsuario", respuestaUsuario },
                            { "respuestaCorrecta", respuestaCorrecta },
                            { "esCorrecta", esCorrecta },
                            { "fecha", DateTime.UtcNow }
                        };

                        var ejercicioDoc = new BsonDocument
                        {
                            { "pregunta", numeroPregunta },
                            { "enunciado", helper.Ejercicio ?? "Sin enunciado" },
                            { "respuestaCorrecta", respuestaCorrecta }
                        };

                        respuestas.Add(respuestaDoc);
                        ejercicios.Add(ejercicioDoc);
                    }
                }

                // Validar que hay datos para guardar
                if (respuestas.Count == 0 || ejercicios.Count == 0)
                {
                    throw new InvalidOperationException("No hay datos para guardar en MongoDB");
                }

                // Calcular tiempo de evaluación
                TimeSpan tiempoEvaluacion = DateTime.UtcNow - DateTime.Parse(Session["InicioEvaluacion"]?.ToString() ?? DateTime.UtcNow.ToString());

                // Guardar en MongoDB con manejo de errores
                MongoDBHelper.GuardarResultado(
                    usuario: usuario,
                    puntaje: puntaje,
                    respuestas: respuestas,
                    ejercicios: ejercicios,
                    tiempoEvaluacion: tiempoEvaluacion,
                    metadata: new BsonDocument
                    {
                        { "navegador", Request.UserAgent },
                        { "ip", Request.UserHostAddress },
                        { "url", Request.Url?.AbsoluteUri }
                    }
                );

                // Mostrar resultados
                MostrarResultados(puntaje, feedbackItems);
            }
            catch (Exception ex)
            {
                // Mostrar error al usuario
                lblResultado.Text = $"<div class='alert alert-danger'>Error: {ex.Message}</div>";
                if (ex.InnerException != null)
                {
                    lblResultado.Text += $"<div class='alert alert-warning'>Detalle: {ex.InnerException.Message}</div>";
                }

                feedbackSection.Visible = true;
                historialSection.Visible = false;
            }
        }

        private void MostrarResultados(int puntaje, List<FeedbackItem> feedbackItems)
        {
            string resultadoClase = puntaje == 3 ? "alert-success" : (puntaje > 0 ? "alert-warning" : "alert-danger");
            double porcentaje = Math.Round((double)puntaje / 3 * 100, 2);

            lblResultado.Text = $@"
                <div class='{resultadoClase}'>
                    <h4>Resultado de la Evaluación</h4>
                    <div class='d-flex justify-content-between'>
                        <span>Puntaje obtenido: <strong>{puntaje} de 3</strong></span>
                        <span>Porcentaje: <strong>{porcentaje}%</strong></span>
                    </div>
                </div>";

            rptFeedback.DataSource = feedbackItems;
            rptFeedback.DataBind();

            feedbackSection.Visible = true;
            historialSection.Visible = false;
        }

        protected void btnNuevosEjercicios_Click(object sender, EventArgs e)
        {
            Session["InicioEvaluacion"] = DateTime.UtcNow.ToString();
            GenerarEjercicios();
            feedbackSection.Visible = false;
            historialSection.Visible = false;
        }

        protected void btnHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = Session["Usuario"]?.ToString() ?? "Invitado";
                var historial = MongoDBHelper.ObtenerHistorialCompleto(usuario, 5);

                if (historial.Count == 0)
                {
                    lblHistorial.Text = "<div class='alert alert-info'>No hay registros históricos para mostrar</div>";
                    historialSection.Visible = true;
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='historial-detallado'>");
                sb.Append("<h4 class='mb-3'>Historial detallado</h4>");
                sb.Append("<div class='accordion' id='historialAccordion'>");

                foreach (var registro in historial)
                {
                    string fecha = registro["fecha"].ToLocalTime().ToString("f");
                    int puntaje = registro["puntaje"].AsInt32;
                    int total = registro["detalle"]["total"].AsInt32;
                    string estado = registro["estado"].AsString;
                    string badgeClass = estado == "completado" ?
                                      (puntaje == total ? "bg-success" : (puntaje > 0 ? "bg-warning" : "bg-danger")) :
                                      "bg-secondary";

                    sb.Append($@"
                    <div class='accordion-item'>
                        <h2 class='accordion-header'>
                            <button class='accordion-button collapsed' type='button' data-bs-toggle='collapse' 
                                    data-bs-target='#collapse{registro["_id"]}' aria-expanded='false'>
                                <span class='me-3'>{fecha}</span>
                                <span class='badge {badgeClass} me-2'>{puntaje}/{total}</span>
                                <span class='small text-muted'>{estado}</span>
                            </button>
                        </h2>
                        <div id='collapse{registro["_id"]}' class='accordion-collapse collapse' 
                             data-bs-parent='#historialAccordion'>
                            <div class='accordion-body'>
                                {GenerarDetalleRegistro(registro)}
                            </div>
                        </div>
                    </div>");
                }

                sb.Append("</div></div>");
                lblHistorial.Text = sb.ToString();
                historialSection.Visible = true;
                feedbackSection.Visible = false;
            }
            catch (Exception ex)
            {
                lblHistorial.Text = $"<div class='alert alert-danger'>Error al cargar el historial: {ex.Message}</div>";
                historialSection.Visible = true;
            }
        }

        private string GenerarDetalleRegistro(BsonDocument registro)
        {
            var ejercicios = registro["ejercicios"].AsBsonArray;
            var respuestas = registro["respuestas"].AsBsonArray;
            var detalle = registro["detalle"].AsBsonDocument;

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='mb-3'>");
            sb.Append($"<p><strong>Tiempo empleado:</strong> {detalle["tiempo"]}</p>");
            sb.Append($"<p><strong>Porcentaje:</strong> {detalle["porcentaje"]}%</p>");
            sb.Append("</div>");

            sb.Append("<table class='table table-sm table-hover'>");
            sb.Append("<thead><tr><th>#</th><th>Ejercicio</th><th>Tu respuesta</th><th>Correcta</th><th>Resultado</th></tr></thead>");
            sb.Append("<tbody>");

            for (int i = 0; i < ejercicios.Count; i++)
            {
                var ejercicio = ejercicios[i].AsBsonDocument;
                var respuesta = respuestas[i].AsBsonDocument;
                bool esCorrecta = respuesta["esCorrecta"].AsBoolean;

                sb.Append($"<tr class='{(esCorrecta ? "table-success" : "table-danger")}'>");
                sb.Append($"<td>{i + 1}</td>");
                sb.Append($"<td>{ejercicio["enunciado"]}</td>");
                sb.Append($"<td>{respuesta["respuestaUsuario"]}</td>");
                sb.Append($"<td>{ejercicio["respuestaCorrecta"]}</td>");
                sb.Append($"<td>{(esCorrecta ? "✅" : "❌")}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody></table>");

            // Mostrar metadatos si existen
            if (registro.Contains("metadata"))
            {
                var metadata = registro["metadata"].AsBsonDocument;
                sb.Append("<div class='mt-3 small text-muted'>");
                sb.Append("<h5>Metadatos:</h5>");
                sb.Append("<ul>");
                foreach (var elemento in metadata)
                {
                    sb.Append($"<li><strong>{elemento.Name}:</strong> {elemento.Value}</li>");
                }
                sb.Append("</ul></div>");
            }

            return sb.ToString();
        }

        // Clases auxiliares para el modelo de vista
        private class EjercicioViewModel
        {
            public int Numero { get; set; }
            public string Enunciado { get; set; }
            public string RespuestaCorrecta { get; set; }
        }

        private class FeedbackItem
        {
            public string Pregunta { get; set; }
            public string Mensaje { get; set; }
            public string ClaseCSS { get; set; }
        }
    }
}