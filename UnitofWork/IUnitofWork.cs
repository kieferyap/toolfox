using Microsoft.EntityFrameworkCore;

// Design pattern: Unit of Work -- groups multiple data-modifying operations into a single transaction. Ensures that all changes are committed or none at all. Similar to DB::beginTransaction() and commit() in Laravel

// Purpose: defining functions for running DB operations
public interface IUnitOfwork : IDisposable
{
  DbContext Context { get; }
  public Task SaveChangesAsync();
}