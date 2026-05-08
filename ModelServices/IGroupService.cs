using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.ModelGeneral;

namespace CanvasRoomDesign.ModelServices
{
    public interface IGroupService
    {
        public Task<StatusMessage<GroupDto>> AddGroup(GroupDto groupDto);

        public Task<StatusMessage<GroupDto>> GetGroupById(Guid id);

        public Task<StatusMessage<List<GroupDto>>> ListGroupBySectionId(Guid id);

        public Task<StatusMessage<GroupDto>> UpdateGroup(GroupDto groupDto);

        public Task<StatusMessage> DeleteGroup(Guid id);

    }
}
