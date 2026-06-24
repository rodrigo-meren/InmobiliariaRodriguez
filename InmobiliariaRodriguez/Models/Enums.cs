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

    public enum Ciudad
    {
        Avellaneda,
        Quilmes,
        SanClementeDelTuyu
    }

    public enum EstadoPropiedad
    {
        Disponible,
        Reservada,
        Vendida,
        Alquilada,
        Suspendida
    }
}