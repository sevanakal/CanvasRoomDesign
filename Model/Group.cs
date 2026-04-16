using System.ComponentModel.DataAnnotations;

namespace CanvasRoomDesign.Model
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string Name { get; set; }
        public string Colour { get; set; }

        public virtual Section Section { get; set; } = null!;
        public virtual ICollection<HallItem> HallItems { get; set; } = new List<HallItem>();
    }
}
