using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebPruebaMongo.Clases
{
    public static class MongoDBHelper
    {
        private static readonly string connectionString = "mongodb://localhost:27017";
        private static readonly string databaseName = "NotacionCientificaDB";
        private static readonly MongoClient client = new MongoClient(connectionString);
        private static readonly IMongoDatabase database = client.GetDatabase(databaseName);

        public static IMongoDatabase GetDatabase()
        {
            return database;
        }

        public static void GuardarResultado(
            string usuario,
            int puntaje,
            List<BsonDocument> respuestas,
            List<BsonDocument> ejercicios,
            TimeSpan tiempoEvaluacion,
            BsonDocument metadata = null)
        {
            // Validación de parámetros
            if (string.IsNullOrWhiteSpace(usuario))
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede estar vacío");
            }

            if (respuestas == null)
            {
                throw new ArgumentNullException(nameof(respuestas), "La lista de respuestas no puede ser nula");
            }

            if (ejercicios == null)
            {
                throw new ArgumentNullException(nameof(ejercicios), "La lista de ejercicios no puede ser nula");
            }

            var collection = database.GetCollection<BsonDocument>("resultadosEvaluacion");

            try
            {
                var resultado = new BsonDocument
                {
                    { "usuario", usuario },
                    { "fecha", DateTime.UtcNow },
                    { "puntaje", puntaje },
                    { "ejercicios", new BsonArray(ejercicios) },
                    { "respuestas", new BsonArray(respuestas) },
                    { "detalle", new BsonDocument
                        {
                            { "correctas", puntaje },
                            { "incorrectas", ejercicios.Count - puntaje },
                            { "total", ejercicios.Count },
                            { "porcentaje", Math.Round((double)puntaje / ejercicios.Count * 100, 2) },
                            { "tiempo", tiempoEvaluacion.ToString(@"hh\:mm\:ss") }
                        }
                    },
                    { "estado", "completado" },
                    { "version", "1.0" }
                };

                // Agregar metadatos si se proporcionaron
                if (metadata != null)
                {
                    resultado.Add("metadata", metadata);
                }

                collection.InsertOne(resultado);
            }
            catch (MongoWriteException mwe)
            {
                throw new ApplicationException("Error de escritura en MongoDB", mwe);
            }
            catch (TimeoutException te)
            {
                throw new ApplicationException("Timeout al conectar con MongoDB", te);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error general al guardar en MongoDB", ex);
            }
        }

        public static List<BsonDocument> ObtenerHistorialCompleto(string usuario, int limite = 5)
        {
            if (string.IsNullOrWhiteSpace(usuario))
            {
                throw new ArgumentException("El usuario no puede estar vacío", nameof(usuario));
            }

            var collection = database.GetCollection<BsonDocument>("resultadosEvaluacion");
            var filtro = Builders<BsonDocument>.Filter.Eq("usuario", usuario);

            var proyeccion = Builders<BsonDocument>.Projection
                .Include("_id")
                .Include("fecha")
                .Include("puntaje")
                .Include("ejercicios")
                .Include("respuestas")
                .Include("detalle")
                .Include("estado")
                .Include("metadata");

            try
            {
                return collection.Find(filtro)
                               .Project(proyeccion)
                               .Sort(Builders<BsonDocument>.Sort.Descending("fecha"))
                               .Limit(limite)
                               .ToList();
            }
            catch (TimeoutException)
            {
                throw new ApplicationException("Tiempo de espera agotado al consultar MongoDB");
            }
            catch (MongoException me)
            {
                throw new ApplicationException($"Error de MongoDB: {me.Message}");
            }
        }

        public static List<BsonDocument> ObtenerTodosLosResultados()
        {
            try
            {
                var collection = database.GetCollection<BsonDocument>("resultadosEvaluacion");
                return collection.Find(new BsonDocument())
                               .Sort(Builders<BsonDocument>.Sort.Descending("fecha"))
                               .ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener todos los resultados", ex);
            }
        }

        public static bool EliminarRegistro(string id)
        {
            try
            {
                var collection = database.GetCollection<BsonDocument>("resultadosEvaluacion");
                var filtro = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var resultado = collection.DeleteOne(filtro);
                return resultado.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al eliminar el registro", ex);
            }
        }

        public static long ContarRegistrosPorUsuario(string usuario)
        {
            try
            {
                var collection = database.GetCollection<BsonDocument>("resultadosEvaluacion");
                var filtro = Builders<BsonDocument>.Filter.Eq("usuario", usuario);
                return collection.CountDocuments(filtro);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al contar registros", ex);
            }
        }
    }
}