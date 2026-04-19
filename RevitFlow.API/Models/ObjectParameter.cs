namespace RevitFlow.API.Models;

public class ObjectParameter
{
    public int Id { get; set; }
    public string? ObjectType { get; set; }
    public int ObjectId { get; set; }
    public string? ParamKey { get; set; }
    public string? ParamValue { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
