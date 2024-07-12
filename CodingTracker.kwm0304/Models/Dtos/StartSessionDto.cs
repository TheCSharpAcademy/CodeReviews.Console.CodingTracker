namespace CodingTracker.kwm0304.Models.Dtos;

public class StartSessionDto
{
  public int DtoId { get; set; }
  public DateTime DtoStartTime { get; set; }
  public StartSessionDto(DateTime time)
  {
    DtoStartTime = time;
  }
}
