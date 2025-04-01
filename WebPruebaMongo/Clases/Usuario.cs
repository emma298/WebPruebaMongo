using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebPruebaMongo.Clases
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("fechaRegistro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [BsonElement("ultimoAcceso")]
        public DateTime UltimoAcceso { get; set; } = DateTime.UtcNow;
    }
}