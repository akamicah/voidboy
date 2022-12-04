namespace DirectoryService.Shared.Attributes;

/// <summary>
/// Register a class as a Scoped service for dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ScopedRegistrationAttribute : Attribute { }