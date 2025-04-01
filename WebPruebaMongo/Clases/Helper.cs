using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebPruebaMongo.Clases
{
    public class NotacionCientificaHelper
    {
        private static readonly Random random = new Random();
        public string RespuestaCorrecta { get; private set; }
        public string Ejercicio { get; private set; }

        public void GenerarEjercicio()
        {
            GenerarEjercicioConversionANotacion();
        }

        private void GenerarEjercicioConversionANotacion()
        {
            int[] exponentes = { -15, -12, -9, -6, -3, -2, -1, 0, 1, 2, 3, 6, 9, 12, 15 };
            int exponente = exponentes[random.Next(exponentes.Length)];
            double coeficiente = Math.Round(random.NextDouble() * 9 + 1, 6);

            if (random.NextDouble() < 0.3) coeficiente *= -1;

            double numero = coeficiente * Math.Pow(10, exponente);
            RespuestaCorrecta = ConvertirANotacionCientificaExacta(numero);
            Ejercicio = $"Convierte el número {FormatearNumero(numero)} a notación científica:";
        }

        public static string FormatearNumero(double numero)
        {
            if (numero == 0) return "0";

            // Para números muy grandes o muy pequeños
            if (Math.Abs(numero) >= 1000000 || (Math.Abs(numero) <= 0.0001 && numero != 0))
            {
                return numero.ToString("0.###############E+0", CultureInfo.InvariantCulture)
                             .Replace("E", " × 10^");
            }

            // Para números en rango normal
            return numero.ToString("#,0.###############", CultureInfo.InvariantCulture);
        }

        private string ConvertirANotacionCientificaExacta(double numero)
        {
            if (numero == 0) return "0 × 10^0";

            int exponente = (int)Math.Floor(Math.Log10(Math.Abs(numero)));
            double coeficiente = numero / Math.Pow(10, exponente);

            // Evitar redondeo mostrando todos los dígitos significativos
            string coeficienteStr = coeficiente.ToString("G15", CultureInfo.InvariantCulture);
            return $"{coeficienteStr} × 10^{exponente}";
        }

        public bool ValidarRespuesta(string respuestaUsuario, string respuestaCorrecta)
        {
            // Normalizar ambas respuestas
            string Normalizar(string input)
            {
                return input.Trim()
                    .ToLowerInvariant()
                    .Replace(" ", "")
                    .Replace("*", "×")
                    .Replace("x", "×")
                    .Replace("e", "×10^")
                    .Replace(",", ".");
            }

            return Normalizar(respuestaUsuario) == Normalizar(respuestaCorrecta);
        }

        public class ConvertidorNotacionCientifica
        {
            public string ConvertirANotacionCientifica(double numero)
            {
                if (numero == 0) return "0 × 10^0";

                int exponente = (int)Math.Floor(Math.Log10(Math.Abs(numero)));
                double coeficiente = numero / Math.Pow(10, exponente);

                // Mostrar todos los dígitos significativos sin redondear
                string coeficienteStr = coeficiente.ToString("G15", CultureInfo.InvariantCulture);
                return $"{coeficienteStr} × 10^{exponente}";
            }

            public double ConvertirANumeroEstandar(double coeficiente, int exponente)
            {
                return coeficiente * Math.Pow(10, exponente);
            }
        }
    }
}