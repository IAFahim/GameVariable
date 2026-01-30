namespace System.Runtime.CompilerServices;

/// <summary>
/// Indicates to the compiler that the .locals init flag should not be set in method headers.
/// </summary>
[AttributeUsage(AttributeTargets.Module
    | AttributeTargets.Class
    | AttributeTargets.Struct
    | AttributeTargets.Interface
    | AttributeTargets.Constructor
    | AttributeTargets.Method
    | AttributeTargets.Property
    | AttributeTargets.Event, Inherited = false)]
internal sealed class SkipLocalsInitAttribute : Attribute
{
}
