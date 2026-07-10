namespace InmobiliariaRodriguez.Web.Models
{
    public enum TipoOperacion
    {
        Venta,
        Alquiler,
        AlquilerTemporario // Fundamental para San Clemente
    }

    public enum TipoPropiedad
    {
        Casa,
        Departamento,
        PH,
        Local,
        Lote,
        Galpon
    }

    public enum Partido
    {
        Avellaneda,
        Quilmes,
        LaCosta
    }

    public enum EstadoPropiedad
    {
        Disponible,
        Reservada,
        Vendida,
        Alquilada,
        Suspendida,
        Retasada,
        Oportunidad
    }
}