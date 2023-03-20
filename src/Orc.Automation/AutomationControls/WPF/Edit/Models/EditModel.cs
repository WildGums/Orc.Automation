namespace Orc.Automation;

using System.Windows;
using System.Windows.Controls;

[ActiveAutomationModel]
public class EditModel : TextBoxBase
{
    public EditModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public string Text { get; set; }
    public int CaretIndex { get; set; }
    public CharacterCasing CharacterCasing { get; set; }
    public int LineCount => _accessor.GetValue<int>();
    public int MaxLength { get; set; }
    public int MaxLines { get; set; }
    public int MinLines { get; set; }
    public string SelectedText { get; set; }
    public int SelectionStart { get; set; }
    public int SelectionLength { get; set; }
    public TextAlignment TextAlignment { get; set; }
    public TextWrapping TextWrapping { get; set; }
}