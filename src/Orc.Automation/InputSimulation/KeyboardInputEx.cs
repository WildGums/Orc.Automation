namespace Orc.Automation
{
    using System.Linq;
    using System.Windows.Input;

    public static class KeyboardInputEx
    {
        public static void Gesture(params Key[] keys)
        {
            foreach (var key in keys)
            {
                KeyboardInput.Press(key);
            }

            foreach (var key in keys.Reverse())
            {
                KeyboardInput.Release(key);
            }
        }

        public static void Copy()
        {
            Gesture(Key.LeftCtrl, Key.C);
        }

        public static void SelectAll()
        {
            Gesture(Key.LeftCtrl, Key.A);
        }
    }
}
