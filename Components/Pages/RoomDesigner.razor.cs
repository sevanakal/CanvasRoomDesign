using Microsoft.AspNetCore.Components.Web;
using CanvasRoomDesign.ModelCanvas;
using Microsoft.AspNetCore.Components;

namespace CanvasRoomDesign.Components.Pages
{
    public partial class RoomDesigner
    {

        /*Parameters*/

        
        private double ToolBoxX = 20, ToolBoxY = 20; //Toolbox ekrandaki x,y koordinatları
        private bool isDraggingToolBox = false; //Toolbox seçili olup olmadığının kontrolu
        private double ToolBoxDragOffsetX = 0, ToolBoxDragOffsetY = 0; //Fare ile nesne seçildiğinde x,y koordinatlarının tutulması

        private double ScreenX = 0, ScreenY = 0;

        private bool isDraggingDesignItem = false;

        public string PrefixName = "A";
        public int PrefixNumber = 1;

        private DesignerState appState = new DesignerState();

        //Seçim alanı için kullanılacak değişkenler
        private bool isSelecting = false;
        private double startX, startY, currentX, currentY;
        private double SelectionBoxX => Math.Min(startX, currentX);
        private double SelectionBoxY => Math.Min(startY, currentY);
        private double SelectionBoxWidth => Math.Abs(currentX - startX);
        private double SelectionBoxHeight => Math.Abs(currentY - startY);
        /*End Parameters*/

        protected override void OnInitialized()
        {
            appState.OnStateChanged += StateHasChanged;
        }

        private void AddNewItem(DesignItemType itemType)
        {
            var (width, height, color, isSellable) = itemType switch
            {
                DesignItemType.armchair => (60, 60, "#999999", true),
                DesignItemType.chair => (60, 60, "#999999", true),
                DesignItemType.table => (150, 75, "#000000", false),
                DesignItemType.stage => (200, 150, "#000000", false),
                _ => (40, 40, "#333333", false)
            };
            DesignItem newItem = new DesignItem
            {
                Name = PrefixName + "-" + PrefixNumber,
                Type = itemType,
                X = appState.GetLastItem()?.X + 80 ?? 200,
                Y = 200,
                Width = width,
                Height = height,
                Color = color,
                IsSellable = isSellable
            };
            PrefixNumber++;
            appState.AddItem(newItem);

        }

        public MarkupString GetItemIcon(DesignItemType itemType) => itemType switch
        {
            DesignItemType.armchair => (MarkupString)@"
                <svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
                                <path d=""M19 9V6a2 2 0 0 0-2-2H7a2 2 0 0 0-2 2v3""></path>
                                <path d=""M3 11v5a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-5a2 2 0 0 0-4 0v2H7v-2a2 2 0 0 0-4 0Z""></path>
                                <path d=""M5 18v2""></path>
                                <path d=""M19 18v2""></path>
                            </svg>
            ",

            DesignItemType.chair=> (MarkupString)@"
                <svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
                                <path d=""M7 4v7""></path>
                                <path d=""M17 4v7""></path>
                                <path d=""M6 11h12v2H6z""></path>
                                <path d=""M8 13v8""></path>
                                <path d=""M16 13v8""></path>
                            </svg>
            ",

            DesignItemType.table=> (MarkupString)@"
                <svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
                                <path d=""M3 8h18v3H3z""></path>
                                <path d=""M5 11v10""></path>
                                <path d=""M19 11v10""></path>
                                <path d=""M9 11v4""></path>
                                <path d=""M15 11v4""></path>
                            </svg>
            ",

            DesignItemType.stage=> (MarkupString)@"
                <svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
                                <path d=""M2 16h20v4H2z""></path>
                                <path d=""M6 16v-4""></path>
                                <path d=""M18 16v-4""></path>
                                <path d=""M4 12h16v-2H4z""></path>
                            </svg>
            ",
            _ => (MarkupString)@"
                <svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"">
                        <rect x=""3"" y=""3"" width=""18"" height=""18"" rx=""2"" ry=""2""></rect>
                        </svg>
            "
        };

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
            
            if (isSelecting)
            {
                currentX = e.ClientX;
                currentY = e.ClientY;
                foreach (var item in appState.Items)
                {
                    bool intersectX = item.X < SelectionBoxX + SelectionBoxWidth && (item.X + item.Width) > SelectionBoxX;
                    bool intersectY = item.Y < SelectionBoxY + SelectionBoxHeight && (item.Y + item.Height) > SelectionBoxY;

                    if (intersectX && intersectY)
                    {
                        appState.SelectItem(item, notify:false);
                    }
                    else
                    {
                        appState.DeSelectItem(item, notify: false);
                    }
                   
                }
            }
            ScreenX = e.ClientX;
            ScreenY = e.ClientY;
        }

        private void OnMouseUpCanvas(MouseEventArgs e) //Canvas üzerinde mouse kontrolü bırakıldığında
        {
            isDraggingToolBox = false;

            isSelecting = false;

            
        }


        

        private void OnCanvasMouseDown(MouseEventArgs e)
        {
            
            isSelecting = true;
            startX = e.ClientX;
            startY = e.ClientY;
            currentX = startX;
            currentY = startY;
            appState.ClearSelection();
        }


        private void DesignItemClick(MouseEventArgs e, DesignItem item) 
        {
            if (!isSelecting)
            {
                appState.ClearSelection();
                appState.SelectItem(item, notify: true);
            }
        }

        private void DesignItemMouseDown(MouseEventArgs e)
        {
            if (e.Button == 0) isDraggingDesignItem = true;
        }

        private void DesignItemMouseUp(MouseEventArgs e)
        {
            isDraggingDesignItem = false;
        }


    }
}
