<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Definicion.aspx.cs" Inherits="WebPruebaMongo.Paginas.Definicion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Definicion de Notacion Cientifica</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css"/>
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
                        <a class="nav-link" href="Ejemplos.aspx">Ejemplos</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="Evaluacion.aspx">Evaluación</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <div class="container mt-4">
        <form id="form1" runat="server">
            <div class="card shadow">
                <div class="card-header">
                    <h1 class="h4">DEFINICIÓN DE NOTACIÓN CIENTÍFICA EN ARITMÉTICA</h1>
                </div>
                <div class="card-body">
                    <blockquote class="blockquote">
                        <p class="mb-0">
                            "La notación científica es una forma de expresar números muy grandes o muy pequeños como un producto de dos factores: 
                            un coeficiente y una potencia de 10."
                            <footer class="blockquote-footer mt-2">Wagner, G., Caicedo, A., & Colorado, H. (2010), p.84</footer>
                        </p>
                    </blockquote>
                    <div class="alert alert-info">
                        <strong>Forma general:</strong> a × 10<sup>n</sup> donde 1 ≤ |a| < 10 y n es un número entero
                    </div>
                </div>
            </div>

            <div class="card shadow">
                <div class="card-header">
                    <h2 class="h4">Ejemplo de Notación Científica</h2>
                </div>
                <div class="card-body">
                    <div class="d-flex align-items-center">
                        <span class="me-2">Número original:</span>
                        <asp:Label ID="lblNumeroOriginal" runat="server" CssClass="number-display me-3"></asp:Label>
                        <span class="me-2">→</span>
                        <span class="me-2">Notación científica:</span>
                        <asp:Label ID="lblNotacionCientifica" runat="server" CssClass="scientific-notation"></asp:Label>
                    </div>                  
                </div>
            </div>

            <div class="card shadow">
                <div class="card-header">
                    <h3 class="h4">BIBLIOGRAFÍA</h3>
                </div>
                <div class="card-body">
                    <p>
                        <strong>Wagner, G., Caicedo, A., & Colorado, H.</strong> (2010). 
                        <em>Principios básicos de aritmética</em>. Pearson Educación.  
                        Recuperado de  
                        <a href="https://www.google.com.mx/books/edition/Principios_B%C3%A1sicos_de_Aritm%C3%A9tica/0uIETOjSShYC?hl=es-419&gbpv=1&dq=notacion+cientifica+aritmetica&pg=PA84&printsec=frontcover" target="_blank">
                            https://www.google.com.mx/books/edition/Principios_B%C3%A1sicos_de_Aritm%C3%A9tica
                        </a>
                    </p>
                </div>
            </div>
        </form>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
