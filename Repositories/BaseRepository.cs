using MySqlConnector;

namespace metalurgicaMVC.Repositories;

public abstract class BaseRepository
{
    /// <summary>
    /// Código de error de MySQL para campos UNIQUE
    /// </summary>
    protected const int ERR_UNIQUE = 1062;
    protected readonly DBContext _context;

    public BaseRepository(DBContext context)
    {
        _context = context;
    }
}