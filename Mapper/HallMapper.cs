using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.Model;
using CanvasRoomDesign.ModelCanvas;

namespace CanvasRoomDesign.Mapper
{
    public static class HallMapper
    {
        public static HallDto ToHallDto(this Hall hall)
        {
            return new HallDto
            {
                Id = hall.Id,
                Name = hall.Name,
                Sections = hall.Sections?.Select(s => s.toSectionDto()).ToList() ?? new List<SectionDto>()
            };
        }

        public static Hall toHallEntity(this HallDto hallDto)
        {
            return new Hall
            {
                Id = hallDto.Id,
                Name = hallDto.Name
            };
        }

        public static SectionDto toSectionDto(this Section section) 
        {
            return new SectionDto
            {
                Id = section.Id,
                HallId = section.HallId,
                Name = section.Name,
                hall = section.hall?.ToHallDto(),
                Groups = section.Groups?.Select(g=>g.toGroupDto()).ToList() ?? new List<GroupDto>(),
                HallItems = section.HallItems?.Select(hi=>hi.toDesignItemDto()).ToList() ?? new List<DesignItem>()
            };
        }

        public static Section toSectionEntity(this SectionDto sectionDto)
        {
            return new Section
            {
                Id = sectionDto.Id,
                HallId = sectionDto.HallId,
                Name = sectionDto.Name
            };
        }

        public static GroupDto toGroupDto(this Group group)
        {
            return new GroupDto
            {
                Id = group.Id,
                SectionId = group.SectionId,
                Name = group.Name
            };
        }

        public static Group toGroupEntity(this GroupDto groupDto)
        {
            return new Group
            {
                Id = groupDto.Id,
                SectionId = groupDto.SectionId,
                Name = groupDto.Name
            };
        }

        public static DesignItem toDesignItemDto(this HallItem designItem)
        {
            return new DesignItem
            {
                Id = designItem.Id,
                SectionId = designItem.SectionId,
                GroupId = designItem.GroupId,
                Name = designItem.Name,
                Type = Enum.Parse<DesignItemType>(designItem.Type, true),
                X = designItem.X,
                Y = designItem.Y,
                Width = designItem.Width,
                Height = designItem.Height,
                Rotation = designItem.Rotation,
                IsSellable = designItem.IsSellable
            };
        }

        public static HallItem toHallItemEntity(this DesignItem designItemDto)
        {
            return new HallItem
            {
                Id = designItemDto.Id,
                SectionId = designItemDto.SectionId,
                GroupId = designItemDto.GroupId,
                Name = designItemDto.Name,
                Type = designItemDto.Type.ToString(),
                X = designItemDto.X,
                Y = designItemDto.Y,
                Width = designItemDto.Width,
                Height = designItemDto.Height,
                Rotation = designItemDto.Rotation,
                IsSellable = designItemDto.IsSellable
            };
        }
    }
}
