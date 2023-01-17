#if TOOLS

#nullable enable

using System;

namespace Unit4Godot;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseAttribute : Attribute
{
    public TestCaseAttribute(params object[] arguments)
    {
        Arguments = arguments;
    }

    public object[] Arguments { get; }
}

#endif // TOOLS