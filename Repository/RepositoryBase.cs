using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// Repository interface
public abstract class RepositoryBase<T> : ControllerBase, IRepository<T> where T : class
{
  protected readonly DbContext _context;
  protected DbSet<T> dbSet;
  private readonly IUnitOfwork _unitOfwork;

  // Constructor
  public RepositoryBase(IUnitOfwork unitOfwork)
  {
    _unitOfwork = unitOfwork;
    _context = unitOfwork.Context;
    dbSet = _unitOfwork.Context.Set<T>();
  }

  // Get Request
  public async Task<ActionResult<IEnumerable<T>>> All()
  {
    var data = await dbSet.ToListAsync();
    return Ok(data);
  }

  // Create Request
  public async Task<ActionResult<T>> Create(T entity)
  {
    dbSet.Add(entity);
    await _unitOfwork.SaveChangesAsync();
    return entity;
  }

  // Update Request
  public async Task<IActionResult> Update(int id, ToolfoxModel entity)
  {
    if (id != entity.Id)
    {
      return BadRequest();
    }

    var existingOrder = await dbSet.FindAsync(id);
    if (existingOrder == null)
    {
      return NotFound();
    }

    _unitOfwork.Context.Entry(existingOrder).CurrentValues.SetValues(entity);

    try
    {
      await _unitOfwork.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      throw;
    }

    return NoContent();
  }

  // Delete Request
  public async Task<IActionResult> Delete(int id)
  {
    var data = await dbSet.FindAsync(id);
    if (data == null)
    {
      return NotFound();
    }

    dbSet.Remove(data);
    await _unitOfwork.SaveChangesAsync();
    return NoContent();
  }

    public Task<IActionResult> Update(int id, T entity)
    {
        throw new NotImplementedException();
    }
}