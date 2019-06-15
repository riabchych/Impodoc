using ImpoDoc.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ImpoDoc.Entities
{
    public abstract class BaseEntity<T> : PropertyChangedNotification
    {
        [Key]
        [DisplayName("Идентификатор")]
        public int Id { get; set; }
        public T ShallowCopy() => (T)MemberwiseClone();
    }
}
