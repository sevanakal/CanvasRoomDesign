using System.ComponentModel.DataAnnotations;

namespace CanvasRoomDesign.Model
{
    public class Section
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public string Name { get; set; } = "";

        public virtual Hall Hall { get; set; } = null!;

        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
        public virtual ICollection<HallItem> HallItems { get; set; } = new List<HallItem>();
    }
}
