using Microsoft.EntityFrameworkCore;
using RestApis.Data;

// Serves as a singular entry point for DB operations
public class UnitOfwork(AppDbContext context) : IUnitOfwork
{
  private readonly AppDbContext _context = context;
  private bool _disposed = false;
  public DbContext Context => _context;

  // For persisting changes to the DB asynchronously
  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  // Ensures proper resource cleanup
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!_disposed)
    {
      if (disposing)
      {
        _context.Dispose();
      }
      _disposed = true;
    }
  }
}