#if TOOLS

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unit4Godot.Internal.Helpers;

namespace Unit4Godot.Internal.Models;

internal class TestInfo
{
    private TestInfo(MethodInfo method, object?[] arguments)
    {
        Method = method;
        Arguments = arguments;
    }

    public MethodInfo Method { get; }

    public object?[] Arguments { get; }

    public static IEnumerable<TestInfo> FromAssembly(Assembly assembly)
    {
        return assembly.ExportedTypes.SelectMany(FromType);
    }

    public static IEnumerable<TestInfo> FromType(Type type)
    {
        return type.GetMethods().SelectMany(FromMethod);
    }

    private static IEnumerable<TestInfo> FromMethod(MethodInfo method)
    {
        if (method.HasAttribute<TestAttribute>())
            return new[] { new TestInfo(method, Array.Empty<object?>()) };

        var attributes = method.GetAttributes<TestCaseAttribute>();
        return attributes.Select(a => new TestInfo(method, a.Arguments));
    }
}

#endif // TOOLS
