using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using toolfox.Models;
using toolfox.Models.ViewModels;

namespace toolfox.Controllers;

/**
* This API uses the following design patterns: Repository and Unit of Work.
* I got this API to work, but I'm not sure how to use it properly at the moment.
* Something about calling the API in JS and using the data...
* ...but this framework uses jQuery, and not Vue or React, so it's not... reactive.
*
* TODO: Is it possible to incorporate Vue with ASP?
* A quick search says that it's possible, but separating it into 2 projects is recommended
*/

[ApiController]
[Route("[controller]")]
public class PlaceApiController : Controller
{
  private readonly IUnitOfwork _unitOfwork;
  IRepository<PlaceModel> placeRepository;


  public PlaceApiController(IUnitOfwork unitOfwork)
  {
    _unitOfwork = unitOfwork;
    placeRepository = new PlaceRepository(_unitOfwork);
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<PlaceModel>>> All()
  { 
    var places = await placeRepository.All();
    return places;
  }

  [HttpPost]
  public async Task<ActionResult<PlaceModel>> Insert(PlaceModel place)
  {
      var places = await placeRepository.Create(place);
      return places;
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
      var places = await placeRepository.Delete(id);
      return places;
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, PlaceModel place)
  {
      var places = await placeRepository.Update(id, place);
      return places;
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
