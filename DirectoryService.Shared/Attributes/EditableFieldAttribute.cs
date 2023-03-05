namespace DirectoryService.Shared.Attributes;

/// <summary>
/// To be applied to fields which through the API should be editable by passing the field name
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class EditableFieldAttribute : Attribute
{
    public EditableFieldAttribute(string fieldName, Permission[] editPermissions)
    {
        FieldName = fieldName;
        EditPermissions = editPermissions;
    }

    public string? FieldName { get; }
    public Permission[] EditPermissions { get; }
}