<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="WebPruebaMongo.Paginas.Registro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Registro - Notación Científica</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" />
    <link href="../css/estilos.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0 text-center">Crear Cuenta</h4>
                    </div>
                    <div class="card-body">
                        <form id="form1" runat="server" class="needs-validation" novalidate>
                            <div class="mb-3">
                                <label for="<%= txtUsuario.ClientID %>" class="form-label">Usuario</label>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" required="true" MaxLength="50"></asp:TextBox>
                                <div class="invalid-feedback">Por favor ingresa un nombre de usuario</div>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtPassword.ClientID %>" class="form-label">Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" required="true" MinLength="6"></asp:TextBox>
                                <div class="invalid-feedback">La contraseña debe tener al menos 6 caracteres</div>
                            </div>

                            <div class="mb-3">
                                <label for="<%= txtConfirmPassword.ClientID %>" class="form-label">Confirmar Contraseña</label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" required="true"></asp:TextBox>
                                <div class="invalid-feedback">Las contraseñas deben coincidir</div>
                            </div>

                            <div class="d-grid gap-2">
                                <asp:Button ID="btnRegistrar" runat="server" Text="Registrarse" 
                                    OnClick="btnRegistrar_Click" CssClass="btn btn-primary" />
                            </div>

                            <div class="mt-3 text-center">
                                <asp:Label ID="lblMensaje" runat="server" CssClass="alert" Visible="false"></asp:Label>
                            </div>

                            <div class="mt-3 text-center">
                                ¿Ya tienes cuenta? <a href="Login.aspx" class="text-primary">Inicia sesión</a>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // Validación del lado del cliente
        (function () {
            'use strict'
            const forms = document.querySelectorAll('.needs-validation')
            
            Array.from(forms).forEach(form => {
                form.addEventListener('submit', event => {
                    if (!form.checkValidity()) {
                        event.preventDefault()
                        event.stopPropagation()
                    }

                    // Validar que las contraseñas coincidan
                    const password = document.getElementById('<%= txtPassword.ClientID %>');
                    const confirmPassword = document.getElementById('<%= txtConfirmPassword.ClientID %>');
                    
                    if (password.value !== confirmPassword.value) {
                        confirmPassword.setCustomValidity("Las contraseñas no coinciden");
                    } else {
                        confirmPassword.setCustomValidity('');
                    }

                    form.classList.add('was-validated')
                }, false)
            })
        })()
    </script>
</body>
</html>