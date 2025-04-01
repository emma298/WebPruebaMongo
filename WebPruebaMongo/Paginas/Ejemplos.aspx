<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ejemplos.aspx.cs" Inherits="WebPruebaMongo.Paginas.Ejemplos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css">
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
                        <a class="nav-link active" href="Evaluacion.aspx">Evaluación</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    
    <div class="container mt-4">
        <form id="form1" runat="server">
            <div class="card shadow">
                <div class="card-header">
                    <h2 class="h4">Conversor de Notación Científica</h2>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtNumero" class="form-label">Ingrese un número:</label>
                        <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control number-input" 
                            placeholder="Ejemplo: 12500000 o 0.000045"></asp:TextBox>
                        <div class="form-text">Puedes usar números muy grandes o muy pequeños</div>
                    </div>
                    <asp:Button ID="btnConvertir" runat="server" Text="Convertir" 
                        CssClass="btn btn-primary" OnClick="btnConvertir_Click" />
                    
                    <div class="result-box mt-3" id="resultContainer" runat="server" visible="false">
                        <h5 class="mb-3">Resultado:</h5>
                        <asp:Label ID="lblResultado" runat="server" CssClass="scientific-example h4"></asp:Label>
                        <asp:Label ID="lblDetalleConversion" runat="server" CssClass="d-block mt-2 text-muted"></asp:Label>
                    </div>
                </div>
            </div>         
        </form>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
