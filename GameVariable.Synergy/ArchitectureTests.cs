using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Xunit;

namespace GameVariable.Synergy;

public class ArchitectureTests
{
    private static readonly string[] ExcludedAssemblies = new[] {
        "GameVariable.Synergy",
        "GameVariable.Synergy.Tests",
        "GameVariable.Benchmarks",
        "GameVariable.Intent.Tests",
        "Variable.Bounded.Tests",
        "Variable.Core.Tests",
        "Variable.Experience.Tests",
        "Variable.Input.Tests",
        "Variable.Inventory.Tests",
        "Variable.Regen.Tests",
        "Variable.Reservoir.Tests",
        "Variable.RPG.Tests",
        "Variable.Timer.Tests"
    };

    private static IEnumerable<Assembly> GetProjectAssemblies()
    {
        var loaded = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var references = typeof(ArchitectureTests).Assembly.GetReferencedAssemblies();
        foreach (var refName in references)
        {
            if ((refName.Name.StartsWith("Variable.") || refName.Name.StartsWith("GameVariable.")) &&
                !ExcludedAssemblies.Contains(refName.Name))
            {
                if (!loaded.Any(a => a.GetName().Name == refName.Name))
                {
                     try { loaded.Add(Assembly.Load(refName)); } catch { }
                }
            }
        }

        return loaded.Where(a => (a.GetName().Name.StartsWith("Variable.") || a.GetName().Name.StartsWith("GameVariable."))
                        && !ExcludedAssemblies.Any(e => a.GetName().Name.Contains(e)));
    }

    private static IEnumerable<Type> GetAllTypes() => GetProjectAssemblies().SelectMany(a => a.GetTypes());

    [Fact]
    public void LayerA_Structs_MustBePureData()
    {
        var structs = GetAllTypes()
            .Where(t => t.IsValueType && !t.IsEnum && !t.IsPrimitive && t.Namespace != null)
            .Where(t => !t.Name.StartsWith("<")) // Exclude compiler generated
            .Where(t => !t.IsNestedPrivate) // Exclude private nested structs
             // Exceptions
            .Where(t => t.FullName != "Variable.RPG.RpgStatSheet")
            .Where(t => t.FullName != "GameVariable.Intent.IntentState");

        foreach (var type in structs)
        {
            // 1. Must be Serializable
            Assert.True(type.IsSerializable, $"{type.FullName} must be [Serializable]");

            // 2. Must be Sequential Layout
            Assert.True(type.IsLayoutSequential, $"{type.FullName} must be [StructLayout(LayoutKind.Sequential)]");

            // 3. No Logic Methods
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                if (method.IsSpecialName) continue; // Properties
                if (method.Name == "Equals" || method.Name == "GetHashCode" || method.Name == "ToString") continue;
                if (method.Name.StartsWith("<")) continue; // Compiler generated

                // IEquatable/IComparable implementation methods are technically logic but usually allowed on data.
                // However, "No Logic" rule is strict.
                // Let's check if it implements interfaces.

                Assert.Fail($"{type.FullName} should not have logic method '{method.Name}'. Layer A is pure data.");
            }
        }
    }

    [Fact]
    public void LayerB_Logic_MustBeStatelessStatic()
    {
        // Identify Logic classes.
        // We assume any static class that is NOT an Extension class (Layer C) is a Logic class.
        // We also exclude "Constants" classes if they exist (Layer A/Shared).

        var staticClasses = GetAllTypes()
            .Where(t => t.IsClass && t.IsAbstract && t.IsSealed) // Static class
            .Where(t => !t.Name.StartsWith("<"));

        foreach (var type in staticClasses)
        {
            if (type.Name.EndsWith("Extensions")) continue;
            if (type.Name.Contains("Constants")) continue; // e.g. MathConstants
            if (type.FullName == "Variable.RPG.RpgStatSheet") continue; // Exception

            // 1. Stateless
            // No static fields unless they are const or readonly.
            // Actually, pure stateless logic shouldn't even have readonly fields unless they are configuration constants.
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
            {
                if (!field.IsLiteral && !field.IsInitOnly && !field.Name.Contains("k__BackingField"))
                {
                    Assert.Fail($"{type.FullName} must be stateless. Field '{field.Name}' is mutable static state.");
                }
            }

            // 2. Primitives Only Inputs/Outputs
            // "Never pass the Struct."
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                if (method.IsSpecialName) continue;
                if (method.Name.StartsWith("<")) continue;

                foreach (var param in method.GetParameters())
                {
                    var paramType = param.ParameterType;
                    if (paramType.IsByRef) paramType = paramType.GetElementType(); // Handle in/out/ref

                    if (!IsAllowedPrimitive(paramType))
                    {
                         Assert.Fail($"{type.FullName}.{method.Name} parameter '{param.Name}' has type '{paramType.Name}'. Layer B must only use primitives/enums, never structs/objects.");
                    }
                }
            }
        }
    }

    private static bool IsAllowedPrimitive(Type t)
    {
        if (t.IsEnum) return true;
        if (t.IsPointer) return true; // Unmanaged/unsafe might use pointers?
        return t == typeof(int) || t == typeof(float) || t == typeof(bool) || t == typeof(double) ||
               t == typeof(long) || t == typeof(byte) || t == typeof(short) || t == typeof(uint) ||
               t == typeof(ulong) || t == typeof(sbyte) || t == typeof(char) || t == typeof(void);
    }

    [Fact]
    public void Naming_MustFollowCleanCodeMandates()
    {
        var types = GetAllTypes();
        foreach (var type in types)
        {
            if (type.Name.StartsWith("<")) continue;

            // Check parameters of methods
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                 foreach (var param in method.GetParameters())
                 {
                     CheckName(param.Name, $"{type.FullName}.{method.Name} param");
                 }
            }

            // Check properties
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                 CheckName(prop.Name, $"{type.FullName}.{prop.Name}");
            }

            // Check fields
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                 if (field.Name.Contains("k__BackingField")) continue;
                 CheckName(field.Name, $"{type.FullName}.{field.Name}");
            }
        }
    }

    private void CheckName(string? name, string context)
    {
        if (string.IsNullOrEmpty(name)) return;

        // 1. Avoid Mental Mapping (Single letters)
        if (name.Length == 1)
        {
            if (name != "x" && name != "y" && name != "z")
            {
                 Assert.Fail($"{context} has name '{name}'. Single letters are banned (except x,y,z).");
            }
        }

        // 2. No Hungarian (basic check)
        // bIsDead, strName, iAge
        if (name.StartsWith("b") && name.Length > 1 && char.IsUpper(name[1]))
             Assert.Fail($"{context} '{name}' looks like Hungarian notation (b...).");

        if (name.StartsWith("str") && name.Length > 3 && char.IsUpper(name[3]))
             Assert.Fail($"{context} '{name}' looks like Hungarian notation (str...).");

        // iAge is tricky because "index" or "item" might be used.
        // But "i" usually means int.
        // Let's check strict hungarian "i" followed by Upper.
        if (name.StartsWith("i") && name.Length > 1 && char.IsUpper(name[1]))
        {
             // Exceptions? maybe "iOS"?
             Assert.Fail($"{context} '{name}' looks like Hungarian notation (i...).");
        }
    }
}
