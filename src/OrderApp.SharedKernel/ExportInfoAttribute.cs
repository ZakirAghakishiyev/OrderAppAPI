namespace OrderApp.SharedKernel;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ExportInfoAttribute : Attribute
{
    public string ContentType { get; }
    public string FileExtension { get; }

    public ExportInfoAttribute(string contentType, string fileExtension)
    {
        ContentType = contentType;
        FileExtension = fileExtension;
    }
}
