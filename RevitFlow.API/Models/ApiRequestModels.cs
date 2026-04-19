namespace RevitFlow.API.Models;

public sealed class CreateProjectRequest
{
    public string? ProjectName { get; set; }
}

public sealed class BulkObjectPointsRequest
{
    public List<ObjectPointCreateDto> Items { get; set; } = new();
}

public sealed class ObjectPointCreateDto
{
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
}

public sealed class BulkObjectLinesRequest
{
    public List<ObjectLineCreateDto> Items { get; set; } = new();
}

public sealed class ObjectLineCreateDto
{
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
}

public sealed class BulkLineStylesRequest
{
    public List<LineStyleCreateDto> Items { get; set; } = new();
}

public sealed class LineStyleCreateDto
{
    public string? StyleName { get; set; }
    public string? Color { get; set; }
    public int Weight { get; set; }
    public string? Pattern { get; set; }
}

public sealed class BulkObjectParametersRequest
{
    public List<ObjectParameterCreateDto> Items { get; set; } = new();
}

public sealed class ObjectParameterCreateDto
{
    public string? ObjectType { get; set; }
    public int ObjectId { get; set; }
    public string? ParamKey { get; set; }
    public string? ParamValue { get; set; }
}
