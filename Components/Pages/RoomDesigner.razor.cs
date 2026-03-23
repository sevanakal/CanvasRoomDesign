using Microsoft.AspNetCore.Components.Web;

namespace CanvasRoomDesign.Components.Pages
{
    public partial class RoomDesigner
    {

        /*Parameters*/

        
        private double ToolBoxX = 20, ToolBoxY = 20; //Toolbox ekrandaki x,y koordinatları
        private bool isDraggingToolBox = false; //Toolbox seçili olup olmadığının kontrolu
        private double ToolBoxDragOffsetX = 0, ToolBoxDragOffsetY = 0; //Fare ile nesne seçildiğinde x,y koordinatlarının tutulması

        private double ScreenX = 0, ScreenY = 0;
        /*End Parameters*/

        
        private void OnToolBoxMouseDown(MouseEventArgs e)//Toolboxda Header tıklandığında x y koordinatlarının girilmesi
        {
            isDraggingToolBox = true;
            ToolBoxDragOffsetX = e.ClientX - ToolBoxX;
            ToolBoxDragOffsetY = e.ClientY - ToolBoxY;
        }

        private void OnToolBoxMouseUp(MouseEventArgs e) 
        {
            
        }

        private void OnMouseMoveCanvas(MouseEventArgs e) //Canvas üzerinde mouse hareketlerinin kontrolü
        {
            if (isDraggingToolBox)
            {
                ToolBoxX = e.ClientX - ToolBoxDragOffsetX;
                ToolBoxY = e.ClientY - ToolBoxDragOffsetY;
            }
            ScreenX = e.ClientX;
            ScreenY = e.ClientY;
        }

        private void OnMouseUpCanvas(MouseEventArgs e)
        {
            isDraggingToolBox = false;
        }







    }
}
