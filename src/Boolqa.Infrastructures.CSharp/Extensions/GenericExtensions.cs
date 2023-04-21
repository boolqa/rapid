namespace Boolqa.Infrastructures.CSharp.Extensions;

public static class GenericExtensions
{
    public static void ThrowIfNull<T>(this T value, string argumentName)
    {
        _ = value ?? throw new ArgumentNullException(argumentName);
    }
}
