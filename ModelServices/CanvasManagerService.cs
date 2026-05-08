using CanvasRoomDesign.DTOs;
using CanvasRoomDesign.Model;
using CanvasRoomDesign.ModelGeneral;

namespace CanvasRoomDesign.ModelServices
{
    public class CanvasManagerService : ICanvasManagerService
    {
        private readonly CanvasDbcontext _context;
        
        private readonly ISectionService _sectionService;
        private readonly IGroupService _groupService;
        private readonly IHallItemService _hallItemService;

        public CanvasManagerService(CanvasDbcontext context, ISectionService sectionService, IGroupService groupService, IHallItemService hallItemService)
        {
            _context = context;
            _sectionService = sectionService;
            _groupService = groupService;
            _hallItemService = hallItemService;
        }

        public async Task<StatusMessage> SaveCanvasAsync(CanvasDesignDto canvasDesignDto)
        {
            StatusMessage statusMessage = new StatusMessage();

            //Burada bir transaction açarak yapacağız. Hernahgi bir işlem yarıda kalırsa veri tabanında tutarsızlık oluşmasın diye.
            using var transaction = await _context.Database.BeginTransactionAsync();

            try 
            { 
                //Silinecekler listesini siliyoruz.
                foreach(var itemId in canvasDesignDto.DeleteDesignItemIds) 
                    await _hallItemService.DeleteHallItem(itemId);

                foreach (var groupId in canvasDesignDto.DeleteGroupIds)
                    await _groupService.DeleteGroup(groupId);

                foreach (var sectionId in canvasDesignDto.DeleteSectionIds)
                    await _sectionService.DeleteSection(sectionId);

                //Eklenecek veya güncellenecek kısmını burada tanımlıyoruz. Id'si olmayanlar yeni eklenecekler, Id'si olanlar güncellenecekler.


            }
            catch (Exception) 
            { 
            
            }

            return statusMessage;
        }
    }
}
