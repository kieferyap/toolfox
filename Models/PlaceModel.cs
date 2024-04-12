namespace toolfox.Models;

public class PlaceModel : ToolfoxModel
{
  // Tells DotNet to auto generate getters and setters for the model
  public int Id {get; set;}

  public string Title {get; set;}

  public string Website {get; set;}

  public string Type {get; set;}

  public string Notes {get; set;}

  public DateTime CreatedAt {get; set;}

  public DateTime UpdatedAt {get; set;}

}
