#if TOOLS

#nullable enable

using Godot;
using Unit4Godot.Internal.Helpers;

namespace Unit4Godot.Internal;

[Tool]
public partial class Unit4GodotPlugin : EditorPlugin
{
    private Control? _panel;

    public override void _EnterTree()
    {
        _panel = UiHelpers.CreatePanel();
        AddControlToBottomPanel(_panel, "Tests");
    }

    public override void _ExitTree()
    {
        RemoveControlFromBottomPanel(_panel);
    }
}

#endif // TOOLS
