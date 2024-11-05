using System;

namespace TransporteUrbano
{
    public class Tarjeta
    {
        public const decimal LimiteSaldo = 9900;
        public const decimal LimiteNegativo = -100;
        public static decimal CostoPasaje = 940; // Costo de pasaje regular
        protected decimal saldo;
        protected DateTime? ultimaFechaViaje;  // Fecha del último viaje

        public Tarjeta(decimal saldoInicial)
        {
            if (saldoInicial <= LimiteSaldo)
            {
                saldo = saldoInicial;
            }
            else
            {
                throw new ArgumentException("El saldo inicial excede el límite permitido.");
            }
        }

        public virtual bool DescontarPasaje()
        {
            if (EsNuevoDia())
            {
                ReiniciarViajesDiarios();
            }

            if (saldo >= CostoPasaje || saldo - CostoPasaje >= LimiteNegativo)
            {
                saldo -= CostoPasaje;
                ultimaFechaViaje = DateTime.Now;  // Actualizar fecha del último viaje
                return true;
            }

            return false; // Saldo insuficiente
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }

        public bool CargarSaldo(decimal monto)
        {
            if (monto >= 2000 && monto <= 9000 && saldo + monto <= LimiteSaldo)
            {
                saldo += monto;
                return true;
            }
            return false;
        }

        // Método para verificar si es un nuevo día
        protected bool EsNuevoDia()
        {
            if (ultimaFechaViaje == null)
            {
                return false;
            }
            return ultimaFechaViaje.Value.Date < DateTime.Now.Date;
        }

        // Método virtual para reiniciar viajes diarios, sobrescrito en las clases derivadas
        protected virtual void ReiniciarViajesDiarios()
        {
        }
    }

    public class MedioBoleto : Tarjeta
    {
        private readonly decimal CostoMedioPasaje = CostoPasaje / 2;
        private int viajesHoy = 0;
        private DateTime? ultimaHoraDeViaje = null;
        private const int MaxViajesPorDia = 4;
        private static readonly TimeSpan IntervaloMinimo = TimeSpan.FromMinutes(5);

        public MedioBoleto(decimal saldoInicial) : base(saldoInicial)
        {
        }

        public override bool DescontarPasaje()
        {
            if (EsNuevoDia())
            {
                ReiniciarViajesDiarios();
            }

            DateTime ahora = DateTime.Now;

            // Verificar si ha pasado el intervalo de 5 minutos
            if (ultimaHoraDeViaje.HasValue && (ahora - ultimaHoraDeViaje.Value) < IntervaloMinimo)
            {
                Console.WriteLine("No se puede realizar otro viaje aún. Debes esperar 5 minutos.");
                return false;
            }

            // Verificar si se ha excedido el límite de viajes con tarifa media
            if (viajesHoy < MaxViajesPorDia)
            {
                if (saldo >= CostoMedioPasaje || saldo - CostoMedioPasaje >= LimiteNegativo)
                {
                    saldo -= CostoMedioPasaje;
                    viajesHoy++;
                    ultimaHoraDeViaje = ahora;
                    ultimaFechaViaje = ahora; // Actualizar fecha del último viaje
                    return true;
                }
            }
            else
            {
                // A partir del quinto viaje, cobrar tarifa completa
                if (saldo >= CostoPasaje || saldo - CostoPasaje >= LimiteNegativo)
                {
                    saldo -= CostoPasaje;
                    viajesHoy++;
                    ultimaHoraDeViaje = ahora;
                    ultimaFechaViaje = ahora; // Actualizar fecha del último viaje
                    return true;
                }
            }

            return false; // Saldo insuficiente
        }

        protected override void ReiniciarViajesDiarios()
        {
            viajesHoy = 0;
        }

        public int ViajesHoy
        {
            get { return viajesHoy; }
        }
    }


    public class FranquiciaCompleta : Tarjeta
    {
        private int viajesGratuitosHoy = 0;
        private const int MaxViajesGratuitos = 2;

        public FranquiciaCompleta(decimal saldoInicial) : base(saldoInicial)
        {
        }

        public override bool DescontarPasaje()
        {
            if (EsNuevoDia())
            {
                ReiniciarViajesDiarios();
            }

            if (viajesGratuitosHoy < MaxViajesGratuitos)
            {
                viajesGratuitosHoy++;
                ultimaFechaViaje = DateTime.Now; // Actualizar fecha del último viaje
                return true; // Viaje gratuito
            }
            else
            {
                // Si ya se alcanzó el límite de viajes gratuitos, cobrar tarifa regular
                return base.DescontarPasaje();
            }
        }

        protected override void ReiniciarViajesDiarios()
        {
            viajesGratuitosHoy = 0;
        }

        public int ViajesGratuitosHoy
        {
            get { return viajesGratuitosHoy; }
        }
    }
}
