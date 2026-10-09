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

    public static void LoguearExcepcion(Exception ex)
    {
        Console.WriteLine($"Tipo de Excepción:\t{ex.GetType()}");
        Console.WriteLine($"Mensaje:\t{ex.Message}");
        Console.WriteLine($"Fuente:\t{ex.Source}");
        Console.WriteLine($"Metodo:\t{ex.TargetSite}");
        Console.WriteLine($"HelpLink:\t{ex.HelpLink}");
    }
}