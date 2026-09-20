namespace metalurgicaMVC.ViewModels;

public enum TipoMensaje
{
    SUCCESS,
    ERROR,
    WARNING
}

public class ModalMensaje
{
    public string Mensaje { get; set; } = string.Empty;
    public TipoMensaje Tipo { get; set; }
}