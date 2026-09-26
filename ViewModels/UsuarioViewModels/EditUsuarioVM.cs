namespace metalurgicaMVC.ViewModels.UsuarioViewModels;

/// <summary>
/// Esta clase es para los datos de la vista
/// Las propiedades UpdatePasswordVM, UpdateDatosVM y UpdateAvatarVM son
///  los modelos que enviaran las peticiones ajax
/// </summary>
public class EditUsuarioVM
{
    public int Id { get; set; }
    public UpdatePasswordVM? UpdatePasswordVM { get; set; }

    public UpdateDatosVM? UpdateDatosVM { get; set; }

    public UpdateAvatarVM? UpdateAvatarVM { get; set; }
}