using System;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace WinHook.Models
{
    [Serializable]
    public class ShorcutSwitch
    {
        public Keys Keys { get; set; }

        public ModifierKeys ModifierKeys { get; set; }

        public ShorcutSwitch(Keys keys, ModifierKeys modifierKeys)
        {
            Keys = keys;
            ModifierKeys = modifierKeys;
        }

        public ShorcutSwitch() { }

        public override string ToString()
        {            
            var shortcutText = new StringBuilder();

            if ((ModifierKeys & ModifierKeys.Control) != 0)
            {
                shortcutText.Append(ModifierKeys.Control);
                shortcutText.Append("+");
            }
            if ((ModifierKeys & ModifierKeys.Shift) != 0)
            {
                shortcutText.Append(ModifierKeys.Shift);
                shortcutText.Append("+");
            }
            if ((ModifierKeys & ModifierKeys.Alt) != 0)
            {
                shortcutText.Append(ModifierKeys.Alt);
                shortcutText.Append("+");
            }
            shortcutText.Append(Keys);

            return shortcutText.ToString();
        }
    }
}
