using ImpoDoc.Converters;
using System.ComponentModel;

namespace ImpoDoc.Models
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Gender
    {
        [Description("Мужской")]
        Male,
        [Description("Женский")]
        Female
    };
}
