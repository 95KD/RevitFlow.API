namespace RevitFlow.API.Models;

public class LineStyle
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string? StyleName { get; set; }
    public string? Color { get; set; }
    public int Weight { get; set; }
    public string? Pattern { get; set; }
}
