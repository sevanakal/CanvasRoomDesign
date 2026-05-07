using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.Mapper;
using CanvasRoomDesign.Model;
using CanvasRoomDesign.ModelGeneral;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CanvasRoomDesign.ModelServices
{
    public class GroupService : IGroupService
    {
        private readonly CanvasDbcontext _context;

        public GroupService(CanvasDbcontext context)
        {
            _context = context;
        }
        public async Task<StatusMessage<GroupDto>> AddGroup(GroupDto groupDto)
        {
            StatusMessage<GroupDto> statusMessage = new StatusMessage<GroupDto>();

            var isGroupExisting = await _context.Groups.AnyAsync(g => g.SectionId == groupDto.SectionId && g.Name == groupDto.Name.Trim());
            
            if (isGroupExisting) 
            { 
                statusMessage.State = false;
                statusMessage.Message = "Group with the same name already exists in this section.";
                statusMessage.Data = null;
            }
            else
            {
                Group groupEntity= groupDto.toGroupEntity();
                _context.Groups.Add(groupEntity);
                await _context.SaveChangesAsync();
                statusMessage.State = true;
                statusMessage.Message = "Group added successfully.";
                statusMessage.Data = groupEntity.toGroupDto();
            }

            return statusMessage;
        }

        public async Task<StatusMessage<GroupDto>> GetGroupById(Guid id)
        {
            StatusMessage<GroupDto> statusMessage = new StatusMessage<GroupDto>();
            
            var group= await _context.Groups.Where(g => g.Id == id).FirstOrDefaultAsync();
            if (group == null)
            {
                statusMessage.State = false;
                statusMessage.Message = "Group not found.";
                statusMessage.Data = null;
            }
            else
            {
                statusMessage.State = true;
                statusMessage.Message = "Group retrieved successfully.";
                statusMessage.Data = group.toGroupDto();
            }
            return statusMessage;
        }

        public async Task<StatusMessage<List<GroupDto>>> ListGroupBySectionId(Guid id)
        {
            StatusMessage<List<GroupDto>> statusMessage = new StatusMessage<List<GroupDto>>();
            var groups = await _context.Groups
        .Where(g => g.SectionId == id)
        .Select(g => g.toGroupDto())
        .ToListAsync();
            if (groups == null || groups.Count == 0)
            {
                statusMessage.State = false;
                statusMessage.Message = "No groups found for the specified section.";
                statusMessage.Data = null;
            }
            else
            {
                statusMessage.State = true;
                statusMessage.Message = "Groups retrieved successfully.";
                statusMessage.Data = groups;
            }
            return statusMessage;
        }

        public async Task<StatusMessage<GroupDto>> UpdateGroup(GroupDto groupDto)
        {
            StatusMessage<GroupDto> statusMessage = new StatusMessage<GroupDto>();
            var existingGroup = await _context.Groups.Where(g => g.Id == groupDto.Id).FirstOrDefaultAsync();
            if (existingGroup == null)
            {
                statusMessage.State = false;
                statusMessage.Message = "Group not found.";
                statusMessage.Data = null;
            }
            else
            {
                var isGroupNameConflict = await _context.Groups.AnyAsync(g => g.SectionId == groupDto.SectionId && g.Name == groupDto.Name.Trim() && g.Id != groupDto.Id);
                if (isGroupNameConflict)
                {
                    statusMessage.State = false;
                    statusMessage.Message = "Another group with the same name already exists in this section.";
                    statusMessage.Data = null;
                }
                else
                {
                    existingGroup.Name = groupDto.Name;
                    await _context.SaveChangesAsync();
                    statusMessage.State = true;
                    statusMessage.Message = "Group updated successfully.";
                    statusMessage.Data = existingGroup.toGroupDto();
                }
            }
            return statusMessage;
        }
    }
}
