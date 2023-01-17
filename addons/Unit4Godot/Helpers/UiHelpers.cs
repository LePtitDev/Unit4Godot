#if TOOLS

#nullable enable

using Godot;
using Unit4Godot.Internal.UI;

namespace Unit4Godot.Internal.Helpers;

internal static class UiHelpers
{
    private static readonly PackedScene TestsPanel = ResourceLoader.Load<PackedScene>("res://addons/Unit4Godot/UI/TestsPanel.tscn");
    private static readonly PackedScene TestsTab = ResourceLoader.Load<PackedScene>("res://addons/Unit4Godot/UI/TestsTab.tscn");

    public static TestsPanel CreatePanel() => (TestsPanel)TestsPanel.Instantiate();

    public static TestsTab CreateTab() => (TestsTab)TestsTab.Instantiate();
}

#endif // TOOLS
