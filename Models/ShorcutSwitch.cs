using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace WinHook.Models
{
    [Serializable]
    //[TypeConverter(typeof(ShorcutSwitchConverter))]
    public class ShorcutSwitch
    {
        //[TypeConverter(typeof(KeysConverter))]
        public Keys Keys { get; set; }

        //[TypeConverter(typeof(ModifierKeysConverter))]
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

    //public class ShorcutSwitchConverter : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    //    }

    //    // Overrides the ConvertFrom method of TypeConverter.
    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        if (!(value is string)) return base.ConvertFrom(context, culture, value);
    //        var v = ((string)value).Split('+');
    //        var keys = (Keys)TypeDescriptor.GetConverter(typeof(Keys)).ConvertFrom(v.Last());
    //        var a = string.Join("+", v.DropLast(1));
    //        var modifiersKeys = (ModifierKeys)TypeDescriptor.GetConverter(typeof(ModifierKeys)).ConvertFromInvariantString(a);
    //        return new ShorcutSwitch(keys, modifiersKeys);
    //    }
    //    // Overrides the ConvertTo method of TypeConverter.
    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (destinationType == typeof(string))
    //        {
    //            return value == null ? "" : value.ToString();
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }
    //}
}
