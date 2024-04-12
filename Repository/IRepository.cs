using Microsoft.AspNetCore.Mvc;
using toolfox.Models;

public interface IRepository<T> where T : class
{
  public Task<ActionResult<IEnumerable<T>>> All();
  public Task<ActionResult<T>> Create(T entity);
  public Task<IActionResult> Update(int id, T entity);
  public Task<IActionResult> Delete(int id);

  // public IActionResult Index();
  // public IActionResult New();
  // public IActionResult Detail();
  // public IActionResult Edit();
}