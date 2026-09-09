using Capa_Datos;

namespace Capa_Logica
{
    // ============================================================
    // Clase: SucursalLogica
    //
    // Responsabilidad:
    // Contener la lógica relacionada con las sucursales.
    //
    // Coordina:
    // - SucursalDatos: obtiene las sucursales desde SQL Server.
    // - SesionActual: controla qué sucursal puede utilizar
    //   el usuario autenticado.
    //
    // Esta clase no ejecuta SQL ni muestra MessageBox.
    // ============================================================
    public class SucursalLogica
    {
        private readonly SucursalDatos sucursalDatos;


        // ========================================================
        // Constructor
        // ========================================================
        public SucursalLogica()
        {
            sucursalDatos = new SucursalDatos();
        }


        // ========================================================
        // Método: ObtenerSucursalesDisponibles
        //
        // Devuelve las sucursales que puede utilizar el usuario
        // autenticado.
        //
        // Administrador:
        //     Puede ver todas las sucursales activas.
        //
        // Gerente/Vendedor:
        //     Solo puede trabajar con su sucursal asignada.
        // ========================================================
        public List<SucursalInfo> ObtenerSucursalesDisponibles()
        {
            List<SucursalInfo> sucursales =
                sucursalDatos.ObtenerActivas();


            // ----------------------------------------------------
            // Administrador
            //
            // Como no tiene una sucursal fija asignada,
            // puede utilizar todas las sucursales activas.
            // ----------------------------------------------------
            if (!SesionActual.IdSucursal.HasValue)
            {
                return sucursales;
            }


            // ----------------------------------------------------
            // Gerente / Vendedor
            //
            // Solo puede utilizar la sucursal que tiene asignada.
            // ----------------------------------------------------
            return sucursales
                .Where(
                    s =>
                        s.IdSucursal ==
                        SesionActual.IdSucursal.Value
                )
                .ToList();
        }


        // ========================================================
        // Método: CambiarSucursalOperativa
        //
        // Cambia la sucursal sobre la que está trabajando
        // actualmente el sistema.
        //
        // idSucursal = NULL:
        //     representa "Todas las sucursales".
        //
        // Devuelve:
        // true  -> cambio permitido.
        // false -> cambio no permitido.
        // ========================================================
        public bool CambiarSucursalOperativa(
            int? idSucursal,
            string nombreSucursal)
        {
            // ----------------------------------------------------
            // Si se intenta seleccionar una sucursal concreta,
            // comprobar que realmente exista y esté disponible
            // para el usuario actual.
            // ----------------------------------------------------
            if (idSucursal.HasValue)
            {
                List<SucursalInfo> disponibles =
                    ObtenerSucursalesDisponibles();


                bool existe =
                    disponibles.Any(
                        s =>
                            s.IdSucursal ==
                            idSucursal.Value
                    );


                if (!existe)
                {
                    return false;
                }
            }


            // ----------------------------------------------------
            // Delegar la regla final a SesionActual.
            //
            // De esta forma la protección queda centralizada
            // también a nivel de sesión.
            // ----------------------------------------------------
            return SesionActual.CambiarSucursalOperativa(
                idSucursal,
                nombreSucursal
            );
        }


        // ========================================================
        // Método: PuedeConsultarTodasLasSucursales
        //
        // Solo los usuarios globales, actualmente Administrador,
        // pueden utilizar la opción "Todas las sucursales".
        // ========================================================
        public bool PuedeConsultarTodasLasSucursales()
        {
            return !SesionActual.IdSucursal.HasValue;
        }
    }
}