namespace Capa_Vistas
{
    // Permite que un formulario avise si tiene
    // cambios pendientes antes de cerrarse.
    public interface IControlaCambios
    {
        bool PuedeCerrar();
    }
}