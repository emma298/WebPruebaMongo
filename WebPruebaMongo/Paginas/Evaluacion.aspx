<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Evaluacion.aspx.cs" Inherits="WebPruebaMongo.Paginas.Evaluacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Evaluacion de Notacion Cientifica</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="/Css/estilos.css" />
</head>
<body>
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">Notación Científica</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" href="Definicion.aspx">Definición</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="Ejemplos.aspx">Ejemplos</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link active" href="Evaluacion.aspx">Evaluación</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <div class="container">
        <form id="form1" runat="server">
            <div class="evaluation-container">
                <h2 class="text-center mb-4">Evaluación de Notación Científica</h2>
                
                <div class="alert alert-info">
                    <strong>Instrucciones:</strong> Convierte cada número a notación científica. Usa el formato: 3.45 × 10<sup>6</sup>
                </div>
                
                <asp:Repeater ID="rptEjercicios" runat="server">
                    <ItemTemplate>
                        <div class="exercise-item">
                            <label class="form-label"><%# Eval("Enunciado") %></label>
                            <asp:TextBox ID="txtRespuesta" runat="server" CssClass="form-control scientific-input" 
                                placeholder="Escribe tu respuesta aquí"></asp:TextBox>
                            <asp:HiddenField ID="hfRespuestaCorrecta" runat="server" Value='<%# Eval("RespuestaCorrecta") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                
                <div class="d-grid gap-2 mt-4">
                    <asp:Button ID="btnEvaluar" runat="server" Text="Evaluar Respuestas" 
                        CssClass="btn btn-primary" OnClick="btnEvaluar_Click" />
                </div>
                
                <div class="feedback-container" id="feedbackSection" runat="server" visible="false">
                    <asp:Label ID="lblResultado" runat="server" CssClass="d-block alert"></asp:Label>
                    <asp:Repeater ID="rptFeedback" runat="server">
                        <ItemTemplate>
                            <div class='alert <%# Eval("ClaseCSS") %> mt-2'>
                                <strong><%# Eval("Pregunta") %></strong><br>
                                <%# Eval("Mensaje") %>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                
                <div class="d-flex justify-content-between mt-4">
                    <asp:Button ID="btnNuevosEjercicios" runat="server" Text="Nuevos Ejercicios" 
                        CssClass="btn btn-outline-secondary" OnClick="btnNuevosEjercicios_Click" />
                    <asp:Button ID="btnHistorial" runat="server" Text="Ver Historial" 
                        CssClass="btn btn-outline-info" OnClick="btnHistorial_Click" />
                </div>
                
                <div class="historial-container" id="historialSection" runat="server" visible="false">
                    <asp:Label ID="lblHistorial" runat="server"></asp:Label>
                </div>
            </div>
        </form>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
