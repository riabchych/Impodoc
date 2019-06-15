using ImpoDoc.Converters;
using System.ComponentModel;

namespace ImpoDoc.Entities
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Gender
    {
        [Description("Чоловіча")]
        Male,
        [Description("Жіноча")]
        Female
    };
}
