#if TOOLS

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Unit4Godot.Internal.Models;

namespace Unit4Godot.Internal.UI;

[Tool]
public partial class TestsTab : Control
{
    private readonly Dictionary<string, TreeItem> _items = new Dictionary<string, TreeItem>();
    private readonly List<TestInstance> _tests = new List<TestInstance>();
    private Texture2D _noStatusIcon = default!;
    private Texture2D _pendingIcon = default!;
    private Texture2D _runningIcon = default!;
    private Texture2D _successIcon = default!;
    private Texture2D _failedIcon = default!;
    private Texture2D _skippedIcon = default!;

    protected Tree Hierarchy { get; private set; } = default!;

    public override void _Ready()
    {
        _noStatusIcon = GetThemeIcon("GuiRadioUnchecked", "EditorIcons");
        _pendingIcon = GetThemeIcon("Pause", "EditorIcons");
        _runningIcon = GetThemeIcon("Play", "EditorIcons");
        _successIcon = GetThemeIcon("StatusSuccess", "EditorIcons");
        _failedIcon = GetThemeIcon("StatusError", "EditorIcons");
        _skippedIcon = GetThemeIcon("GuiVisibilityHidden", "EditorIcons");

        Hierarchy = (Tree)GetNode("Body/SplitContainer/List");
        Hierarchy.Columns = 2;
        Hierarchy.SetColumnTitle(0, "Name");
        Hierarchy.SetColumnTitle(1, "Status");
        Hierarchy.ColumnTitlesVisible = true;

        SetToolbarButton("PlayButton", "Play");
        SetToolbarButton("StopButton", "Stop");
        SetToolbarButton("BuildButton", "Bake");
        SetToolbarButton("TestsCount", "ClassList");
        SetToolbarButton("SuccessCount", "CheckBox");
        SetToolbarButton("ErrorCount", "Error");
    }

    internal void UpdateTests(IEnumerable<TestInstance> tests)
    {
        _tests.Clear();
        ClearItems();
        foreach (var test in tests)
        {
            _tests.Add(test);
            UpdateTest(test);
        }
    }

    private void UpdateTest(TestInstance test)
    {
        var item = GetOrCreateItem(GetTestName(test));
        item.SetIcon(0, GetStatusIcon(test.Status));
    }

    private void UpdateGroup(string path)
    {
        var pattern = path + '.';
        var children = _tests.Where(x => GetTestName(x).StartsWith(pattern, StringComparison.Ordinal));
        var status = TestStatus.NoStatus;
        foreach (var test in children)
        {
            if (status < test.Status)
                status = test.Status;
        }

        var item = GetOrCreateItem(path);
        item.SetIcon(0, GetStatusIcon(status));
    }

    private void SetToolbarButton(string name, string iconName)
    {
        var icon = GetThemeIcon(iconName, "EditorIcons");
        var button = (Button)GetNode($"Toolbar/Stack/{name}");
        button.Icon = icon;
    }

    protected void ClearItems()
    {
        _items.Clear();
        Hierarchy.Clear();
    }

    protected TreeItem GetOrCreateItem(string path)
    {
        var count = path.TakeWhile(c => c != '(').Count(c => c == '.');
        return GetOrCreateItem(path.Split('.', count + 1))!;
    }

    protected TreeItem[] GetChildrenItems(string path)
    {
        var pattern = path + ".";
        return _items
            .Where(x => x.Key.StartsWith(pattern, StringComparison.Ordinal) && x.Key.IndexOf('.', pattern.Length) < 0)
            .Select(x => x.Value)
            .ToArray();
    }

    private TreeItem? GetOrCreateItem(string[] path)
    {
        if (path.Length == 0)
            return null;

        var strPath = string.Join('.', path);
        if (_items.TryGetValue(strPath, out var item))
            return item;

        var parent = GetOrCreateItem(path.Take(path.Length - 1).ToArray());
        item = Hierarchy.CreateItem(parent);
        _items[strPath] = item;

        item.SetIcon(0, _noStatusIcon);
        item.SetText(0, path[^1]);
        return item;
    }

    private void SetTestsCount(int count) => SetCounter("TestsCount", count);

    private void SetSuccessCount(int count) => SetCounter("SuccessCount", count);

    private void SetErrorCount(int count) => SetCounter("ErrorCount", count);

    private void SetCounter(string name, int count)
    {
        var button = (Button)GetNode($"Toolbar/Stack/{name}");
        button.Text = count.ToString();
    }

    private Texture2D GetStatusIcon(TestStatus status)
    {
        return status switch
        {
            TestStatus.Pending => _pendingIcon,
            TestStatus.Running => _runningIcon,
            TestStatus.Success => _successIcon,
            TestStatus.Failed => _failedIcon,
            TestStatus.Skipped => _skippedIcon,
            _ => _noStatusIcon
        };
    }

    private static string GetTestName(TestInstance test)
    {
        var method = test.Info.Method;
        return test.Info.Arguments.Length == 0
            ? $"{method.DeclaringType!.FullName}.{method.Name}"
            : $"{method.DeclaringType!.FullName}.{method.Name}.{method.Name}({FormatArguments(test.Info.Arguments)})";

        static string FormatArguments(object?[] arguments)
        {
            return string.Join(", ", arguments.Select(FormatArgument));
        }

        static string? FormatArgument(object? argument)
        {
            if (argument == null)
                return "null";

            if (argument is string)
                return $"\"{argument}\"";

            return argument.ToString();
        }
    }
}

#endif // TOOLS
