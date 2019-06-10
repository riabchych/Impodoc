using ImpoDoc.Converters;
using System.ComponentModel;

namespace ImpoDoc.Entities
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
