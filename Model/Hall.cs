using System.ComponentModel.DataAnnotations;

namespace CanvasRoomDesign.Model
{
    public class Hall
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Section> Sections { get; set; }=new  List<Section>();

    }
}
