using CanvasRoomDesign.ModelGeneral;
using System.ComponentModel.DataAnnotations;

namespace CanvasRoomDesign.Model
{
    public class Section : ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public string Name { get; set; } = "";
        public bool IsDeleted { get; set; } = false;

        public virtual Hall hall { get; set; } = null!;

        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ICollection<HallItem> HallItems { get; set; } = new List<HallItem>();
    }
}
