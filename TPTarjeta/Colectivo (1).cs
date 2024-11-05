using System;

namespace TransporteUrbano
{
    public class Colectivo
    {
        private string linea;
        private static int viajeContador = 1;

        public Colectivo(string linea)
        {
            this.linea = linea;
        }

        public string Linea
        {
            get { return linea; }
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            if (tarjeta is FranquiciaCompleta franquiciaCompleta)
            {
                if (franquiciaCompleta.DescontarPasaje())
                {
                    decimal montoCobrado = (franquiciaCompleta.ViajesGratuitosHoy <= 2) ? 0 : Tarjeta.CostoPasaje;
                    return new Boleto(montoCobrado, "Pasaje Franquicia", linea, tarjeta.ObtenerSaldo(), viajeContador++);
                }
            }
            else if (tarjeta is MedioBoleto medioBoleto)
            {
                if (medioBoleto.DescontarPasaje())
                {
                    decimal montoCobrado = (medioBoleto.ViajesHoy <= 4) ? Tarjeta.CostoPasaje / 2 : Tarjeta.CostoPasaje;
                    return new Boleto(montoCobrado, "Pasaje Medio Boleto", linea, tarjeta.ObtenerSaldo(), viajeContador++);
                }
            }
            else
            {
                if (tarjeta.DescontarPasaje())
                {
                    return new Boleto(Tarjeta.CostoPasaje, "Pasaje Regular", linea, tarjeta.ObtenerSaldo(), viajeContador++);
                }
            }

            return null; // Si no se puede descontar el pasaje
        }
    }
}
