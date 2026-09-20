namespace metalurgicaMVC.Repositories;

public abstract class BaseRepository
{
    protected readonly DBContext _context;

    public BaseRepository(DBContext context)
    {
        _context = context;
    }
}