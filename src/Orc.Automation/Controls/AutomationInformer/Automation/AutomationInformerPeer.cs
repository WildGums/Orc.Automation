namespace Orc.Automation
{
    public class AutomationInformerPeer : ControlRunMethodAutomationPeerBase<AutomationInformer>
    {
        public AutomationInformerPeer(AutomationInformer owner)
            : base(owner)
        {
        }

        //[AutomationMethod]
        //public void SetAutomationId(string parentId, string partName, string id)
        //{
        //    MessageBox.Show($"{parentId}---{partName}---{id}");

        //    var targetControl = Control.FindVisualDescendant(x => Equals((x as FrameworkElement)?.GetValue(AutomationProperties.AutomationIdProperty), parentId));

        //    MessageBox.Show($"targetControl = {targetControl}");

        //    var part = targetControl.GetChildren().FirstOrDefault(); //targetControl?.FindVisualDescendantByType<Border>();
        //    MessageBox.Show($"part = {part}");

        //    part?.SetCurrentValue(AutomationProperties.AutomationIdProperty, id);
        //    part?.SetCurrentValue(AutomationProperties.NameProperty, id);
        //    MessageBox.Show($"{part?.GetValue(AutomationProperties.AutomationIdProperty) ?? "null"}");
        //}
    }
}
