using Microsoft.EntityFrameworkCore;
using RestApis.Data;
using toolfox.Models;

public class PlaceRepository : RepositoryBase<PlaceModel>
{
  public PlaceRepository(IUnitOfwork unitOfwork) : base(unitOfwork)
  {
  }
}