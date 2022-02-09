namespace Orc.Automation
{
    using System.Windows.Input;

    public static class KeyboardInputEx
    {
        public static void Copy()
        {
            KeyboardInput.Press(Key.LeftCtrl);
            KeyboardInput.Press(Key.C);
            KeyboardInput.Release(Key.C);
            KeyboardInput.Release(Key.LeftCtrl);
        }

        public static void SelectAll()
        {
            KeyboardInput.Press(Key.LeftCtrl);
            KeyboardInput.Press(Key.A);
            KeyboardInput.Release(Key.A);
            KeyboardInput.Release(Key.LeftCtrl);
        }
    }
}
