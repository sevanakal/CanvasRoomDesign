namespace CanvasRoomDesign.DTOs
{
    public class HallDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public List<SectionDto> Sections { get; set; } = new List<SectionDto>();
    }
}
