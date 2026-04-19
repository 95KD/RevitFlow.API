namespace RevitFlow.API.Models;

public class ObjectPoint
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string? ElementId { get; set; }
    public string? ObjectType { get; set; }
    public string? FamilyName { get; set; }
    public string? TypeName { get; set; }
    public string? LevelName { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double FacingX { get; set; }
    public double FacingY { get; set; }
    public double FacingZ { get; set; }
    public double HandX { get; set; }
    public double HandY { get; set; }
    public double HandZ { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
