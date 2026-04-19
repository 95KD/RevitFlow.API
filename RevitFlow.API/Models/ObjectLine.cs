namespace RevitFlow.API.Models;

public class ObjectLine
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string? ElementId { get; set; }
    public string? ObjectType { get; set; }
    public string? FamilyName { get; set; }
    public string? TypeName { get; set; }
    public string? LevelName { get; set; }
    public double StartX { get; set; }
    public double StartY { get; set; }
    public double StartZ { get; set; }
    public double EndX { get; set; }
    public double EndY { get; set; }
    public double EndZ { get; set; }
    public int LineStyleId { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
