using CanvasRoomDesign.ModelGeneral;
using System.ComponentModel.DataAnnotations;

namespace CanvasRoomDesign.Model
{
    public class Hall : ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Section> Sections { get; set; }=new  List<Section>();

    }
}
