using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using metalurgicaMVC.Utils;
using Microsoft.EntityFrameworkCore;

namespace metalurgicaMVC.Repositories;

public class ClienteRepository : BaseRepository, IClienteRepository
{
    public ClienteRepository(DBContext context) : base(context) { }

    public async Task<int> CountAsync(FiltrosCliente filtros)
    {
        try
        {
            var clientes = _context.Clientes;

            if (!string.IsNullOrWhiteSpace(filtros.Busqueda))
            {
                return await clientes
                    .CountAsync(c => EF.Functions.Like(c.Nombre, $"{filtros.Busqueda}%") || EF.Functions.Like(c.Apellido, $"{filtros.Busqueda}%"));
            }
            else
                return await clientes.CountAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("No se pasaron valores");
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<int> CreateAsync(Cliente cliente)
    {
        try
        {
            cliente.Id = 0; // por si las moscas
            _context.Clientes.Add(cliente);
            return await _context.SaveChangesAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo guardar el cliente", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Cliente? cliente = await GetByIdAsync(id);

        try
        {
            cliente?.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo eliminar el cliente", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al eliminar el cliente", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<Cliente?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Clientes.FindAsync(id);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<List<Cliente>> ListAsync(int limit, int offset)
    {
        try
        {
            IQueryable<Cliente> clientes =  _context.Clientes;

            if (limit > 0 && offset > 0)
                clientes = clientes.Skip(offset).Take(limit);

            return await clientes.ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("No se pasaron valores");
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<List<Cliente>> ListAsync(FiltrosCliente filtros)
    {
        try
        {
            IQueryable<Cliente> clientes = _context.Clientes;

            if (!string.IsNullOrWhiteSpace(filtros.Busqueda))
            {
                clientes = clientes
                    .Where(c => EF.Functions.Like(c.Nombre, $"{filtros.Busqueda}%") || EF.Functions.Like(c.Apellido, $"{filtros.Busqueda}%"));
            }

            if (filtros.Limit > 0 && filtros.Offset() >= 0)
                clientes = clientes.Skip(filtros.Offset()).Take(filtros.Limit);

            return await clientes.ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("No se pasaron valores");
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }

    public async Task<bool> UpdateAsync(Cliente clienteActualizado)
    {
        Cliente? cliente = await GetByIdAsync(clienteActualizado.Id);
        
        try
        {
            if (cliente != null)
            {
                cliente.CopyValuesFrom(clienteActualizado);
                return (await _context.SaveChangesAsync()) > 0;
            }
            else
                throw new ClientException("El cliente no existe");
        }
        catch (OperationCanceledException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo completar la operación", ex);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("No se pudo actualizar el cliente", ex);
        }
        catch (DbUpdateException ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Error al actualizar el cliente", ex);
        }
        catch (Exception ex)
        {
            Util.LoguearExcepcion(ex);
            throw new AppException("Ocurrió un error inesperado", ex);
        }
    }
}