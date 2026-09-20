using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace metalurgicaMVC.Utils;

public class Util
{
    public static string ModelStateViewError(ModelStateDictionary modelState)
    {
        string errorMsg = string.Empty;
        foreach (var estado in modelState)
        {
            var campo = estado.Key;
            foreach (var error in estado.Value.Errors)
            {
                errorMsg += @$"{error.ErrorMessage}\n";
            }
        }
        return errorMsg;
    }

    public static Dictionary<string, string> ModelStateJsonError(ModelStateDictionary modelState)
    {
        Dictionary<string, string> errores = new();
        foreach (var estado in modelState)
        {
            var campo = estado.Key;
            string mensajes = string.Empty;
            foreach (var error in estado.Value.Errors)
            {
                mensajes += $"- {error.ErrorMessage}";
            }

            errores.TryAdd(campo, mensajes);
        }
        return errores;
    }

    public static void LoguearExcepcion(Exception ex)
    {
        Console.WriteLine($"Tipo de Excepción:\t{ex.GetType()}");
        Console.WriteLine($"Mensaje:\t{ex.Message}");
        Console.WriteLine($"Fuente:\t{ex.Source}");
        Console.WriteLine($"Metodo:\t{ex.TargetSite}");
        Console.WriteLine($"HelpLink:\t{ex.HelpLink}");
    }
}