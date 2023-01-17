#if TOOLS

#nullable enable

using System;
using System.Linq;
using Godot;
using Unit4Godot.Internal.Helpers;
using Unit4Godot.Internal.Models;

namespace Unit4Godot.Internal.UI;

[Tool]
public partial class TestsPanel : MarginContainer
{
	public override void _Ready()
	{
		try
		{
			var styleBox = GetThemeStylebox("BottomPanel", "EditorStyles");
			AddThemeConstantOverride("margin_top", (int)-styleBox.ContentMarginTop);
			AddThemeConstantOverride("margin_left", (int)-styleBox.ContentMarginLeft);
			AddThemeConstantOverride("margin_right", (int)-styleBox.ContentMarginRight);
			var tab = UiHelpers.CreateTab();
			var container = (TabContainer)GetNode("TabContainer");
			tab.Name = "All tests";
			container.AddChild(tab);
			tab.UpdateTests(TestInfo.FromAssembly(typeof(TestsPanel).Assembly).Select(x => new TestInstance(x)));
		}
		catch (Exception e)
		{
			GD.PrintErr(e);
		}
	}
}

#endif // TOOLS
