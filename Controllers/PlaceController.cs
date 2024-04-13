using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using toolfox.Models;
using toolfox.Models.ViewModels;

namespace toolfox.Controllers;

public class PlaceController : Controller
{
  private readonly ILogger<PlaceController> _logger;

  private readonly IConfiguration _configuration;

  private readonly IUnitOfwork? _unitOfwork;
  IRepository<PlaceModel>? placeRepository;

  public PlaceController(ILogger<PlaceController> logger, IConfiguration configuration)
  {
    _logger = logger;
    _configuration = configuration;
  }

  public IActionResult Index()
  {
    // View() matches the name of the function, that is, "Index"...
    // ...to the filename within Views, that is, "Index.cshtml"
    var placeListViewModel = GetAllPlaces();
    return View(placeListViewModel);
  }

  public IActionResult New()
  {
    return View();
  }

  public IActionResult Detail(int id)
  {
    var place = GetPlaceById(id);
    var placeViewModel = new PlaceViewModel{Place = place};
    return View(placeViewModel);
  } 

  public IActionResult Edit(int id)
  {
    var place = GetPlaceById(id);
    var placeViewModel = new PlaceViewModel{Place = place};
    return View(placeViewModel);
  }

  [HttpPost]
  public JsonResult Delete(int id)
  {
    // Connect to the DB (ASP will autoclean apparently)
    using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ToolfoxDataContext")))
    {
      using (var command = connection.CreateCommand()) {
        connection.Open();
        // Construct the query
        command.CommandText = $"DELETE FROM place WHERE Id = '{id}'";

        // Run the query
        try {
          command.ExecuteNonQuery();
        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }
      }
    }

    return Json(new Object{});
  }

  
  // See PlaceApiController.cs
  public ActionResult Insert(PlaceModel place)
  {
    place.CreatedAt = DateTime.Now;
    place.UpdatedAt = DateTime.Now;

    // Connect to the DB
    using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ToolfoxDataContext")))
    {
      using (var command = connection.CreateCommand()) {
        connection.Open();
        // Construct the query
        command.CommandText = $"INSERT INTO place (title, website, type, notes, createdat, updatedat) VALUES ('{place.Title}', '{place.Website}', '{place.Type}', '{place.Notes}', '{place.CreatedAt}', '{place.UpdatedAt}')";

        // Run the query
        try {
          command.ExecuteNonQuery();
        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }
      }
    }

    return RedirectToAction(nameof(Index));
  }

  internal PlaceModel GetPlaceById(int id) 
  {
    PlaceModel place = new();

    // Connect to the DB
    using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ToolfoxDataContext")))
    {
      using (var command = connection.CreateCommand()) {
        connection.Open();
        // Construct the query
        command.CommandText = $"SELECT * FROM place WHERE Id = '{id}'";

         // Run the query
        using (var reader = command.ExecuteReader()) {
          // If there are rows, stuff it into the list
          if (reader.HasRows) {
            while (reader.Read()) {
              place.Id = reader.GetInt32(0);
              place.Title = reader.GetString(1);
              place.Website = reader.GetString(2);
              place.Type = reader.GetString(3);
              place.Notes = reader.GetString(4);
              place.CreatedAt = reader.GetDateTime(5);
              place.UpdatedAt = reader.GetDateTime(6);
            }
          } else {
            return place;
          }
        }
      }
    }

    return place;
  }

  internal PlaceViewModel GetAllPlaces()
  {
    List<PlaceModel> placeList = new();

    // Connect to the DB
    using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ToolfoxDataContext")))
    {
      using (var command = connection.CreateCommand()) {
        connection.Open();
        // Construct the query
        command.CommandText = $"SELECT * FROM place";

        // Run the query
        using (var reader = command.ExecuteReader()) {
          // If there are rows, stuff it into the list
          if (reader.HasRows) {
            while (reader.Read()) {
              placeList.Add(
                new PlaceModel {
                  Id = reader.GetInt32(0),
                  Title = reader.GetString(1),
                  Website = reader.GetString(2),
                  Type = reader.GetString(3),
                  Notes = reader.GetString(4),
                  CreatedAt = reader.GetDateTime(5),
                  UpdatedAt = reader.GetDateTime(6),
                }
              );
            }
          } else {
            return new PlaceViewModel {
              PlaceList = placeList
            };
          }
        }
      }
    }

    return new PlaceViewModel
    {
      PlaceList = placeList
    };
  }

  public ActionResult Update(PlaceModel place)
  {
    place.UpdatedAt = DateTime.Now;
    
    // Connect to the DB
    using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ToolfoxDataContext")))
    {
      using (var command = connection.CreateCommand()) {
        connection.Open();
        // Construct the query
        command.CommandText = $"UPDATE place SET title = '{place.Title}', website = '{place.Website}', type = '{place.Type}', notes = '{place.Notes}', updatedAt = '{place.UpdatedAt}' WHERE Id = {place.Id}";

        // Run the query
        try {
          command.ExecuteNonQuery();
        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }
      }
    }

    return RedirectToAction(nameof(Index));
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
