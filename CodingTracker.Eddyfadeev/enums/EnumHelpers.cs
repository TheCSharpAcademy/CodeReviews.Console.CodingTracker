namespace CodingTracker.enums;

public static class EnumHelpers
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MethodAttribute(string methodName) : Attribute
    {
        public string MethodName { get; } = methodName;
    }
}