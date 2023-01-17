#if TOOLS

#nullable enable

namespace Unit4Godot.Internal.Models;

internal class TestInstance
{
    public TestInstance(TestInfo info)
    {
        Info = info;
    }

    public TestInfo Info { get; }

    public TestStatus Status { get; private set; } = TestStatus.NoStatus;

    public string? Output { get; private set; } = null;

    public void Run()
    {

    }
}

#endif // TOOLS
