using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MongoDB.Driver;
using WebPruebaMongo.Clases;

namespace WebPruebaMongo.Clases
{
    public static class AuthHelper
    {
        private const string UsersCollection = "usuarios";
        private const int TokenExpirationDays = 30;

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        public static (bool success, string message) RegistrarUsuario(string nombreUsuario, string password, string confirmPassword)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                    return (false, "El nombre de usuario no puede estar vacío");

                if (string.IsNullOrWhiteSpace(password))
                    return (false, "La contraseña no puede estar vacía");

                if (password != confirmPassword)
                    return (false, "Las contraseñas no coinciden");

                if (password.Length < 6)
                    return (false, "La contraseña debe tener al menos 6 caracteres");

                var db = MongoDBHelper.GetDatabase();
                var users = db.GetCollection<Usuario>("usuarios");

                // Verificar si el usuario ya existe
                if (users.Find(u => u.Nombre == nombreUsuario).Any())
                    return (false, "El nombre de usuario ya está en uso");

                // Crear hash de la contraseña
                var hashedPassword = HashPassword(password);

                var nuevoUsuario = new Usuario
                {
                    Nombre = nombreUsuario.Trim(),
                    Password = hashedPassword,
                    FechaRegistro = DateTime.UtcNow,
                    UltimoAcceso = DateTime.UtcNow
                };

                users.InsertOne(nuevoUsuario);

                return (true, "Usuario registrado con éxito");
            }
            catch (Exception ex)
            {
                return (false, $"Error al registrar usuario: {ex.Message}");
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Autentica un usuario en el sistema
        /// </summary>
        public static (bool success, string message, Usuario usuario) AutenticarUsuario(string nombreUsuario, string password, bool recordarSesion = false)
        {
            try
            {
                var db = MongoDBHelper.GetDatabase();
                var users = db.GetCollection<Usuario>(UsersCollection);

                // Buscar usuario por nombre
                var usuario = users.Find(u => u.Nombre == nombreUsuario.Trim()).FirstOrDefault();

                if (usuario == null || !VerifyPassword(password, usuario.Password))
                    return (false, "Usuario o contraseña incorrectos", null);

                // Actualizar último acceso
                var update = Builders<Usuario>.Update.Set(u => u.UltimoAcceso, DateTime.UtcNow);
                users.UpdateOne(u => u.Id == usuario.Id, update);

                // Generar token si se seleccionó "Recordar sesión"
                if (recordarSesion)
                {
                    string token = GenerateToken(usuario);
                    SetAuthCookies(usuario.Nombre, token);
                }

                return (true, "Autenticación exitosa", usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Error al autenticar: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Verifica si hay una sesión recordada válida
        /// </summary>
        public static Usuario VerificarSesionRecordada(HttpRequest request)
        {
            try
            {
                if (request.Cookies["UsuarioRecordado"] == null || request.Cookies["TokenSesion"] == null)
                    return null;

                var db = MongoDBHelper.GetDatabase();
                var users = db.GetCollection<Usuario>(UsersCollection);

                var usuario = users.Find(u => u.Nombre == request.Cookies["UsuarioRecordado"].Value).FirstOrDefault();

                if (usuario != null && ValidateToken(usuario, request.Cookies["TokenSesion"].Value))
                {
                    return usuario;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario
        /// </summary>
        public static void CerrarSesion(HttpResponse response)
        {
            // Eliminar cookies de autenticación
            response.Cookies["UsuarioRecordado"].Expires = DateTime.Now.AddDays(-1);
            response.Cookies["TokenSesion"].Expires = DateTime.Now.AddDays(-1);
        }

        #region Métodos Privados

        

        private static bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashOfInput = HashPassword(inputPassword);
            return hashOfInput == storedHash;
        }

        private static string GenerateToken(Usuario usuario)
        {
            using (var sha256 = SHA256.Create())
            {
                var rawToken = $"{usuario.Id}{usuario.Nombre}{DateTime.UtcNow.Ticks}";
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static bool ValidateToken(Usuario usuario, string token)
        {
            var expectedToken = GenerateToken(usuario);
            return token == expectedToken;
        }

        private static void SetAuthCookies(string nombreUsuario, string token)
        {
            HttpContext.Current.Response.Cookies["UsuarioRecordado"].Value = nombreUsuario;
            HttpContext.Current.Response.Cookies["UsuarioRecordado"].Expires = DateTime.Now.AddDays(TokenExpirationDays);

            HttpContext.Current.Response.Cookies["TokenSesion"].Value = token;
            HttpContext.Current.Response.Cookies["TokenSesion"].Expires = DateTime.Now.AddDays(TokenExpirationDays);
        }

        #endregion
    }
}