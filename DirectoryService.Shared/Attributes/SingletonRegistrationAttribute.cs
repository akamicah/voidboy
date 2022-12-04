namespace DirectoryService.Shared.Attributes;

/// <summary>
/// Register a class as a Singleton service for dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class SingletonRegistrationAttribute : Attribute { }