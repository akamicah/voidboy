namespace DirectoryService.Shared.Attributes;

/// <summary>
/// Register a class as a Transient service for dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class TransientDependencyAttribute : Attribute { }