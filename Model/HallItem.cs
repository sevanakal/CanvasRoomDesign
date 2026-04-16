using System.ComponentModel.DataAnnotations;

namespace CanvasRoomDesign.Model
{
    public class HallItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public Guid? GroupId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; }
        public string Colour { get; set; }
        public bool IsSellable { get; set; }

        public virtual Section Section { get; set; } = null!;
        public virtual Group? Group { get; set; }
    }
}
