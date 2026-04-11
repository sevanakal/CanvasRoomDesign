using Microsoft.AspNetCore.Components.Web;
using CanvasRoomDesign.ModelCanvas;
using CanvasRoomDesign.UIServices;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Eventing.Reader;
using System.Transactions;

namespace CanvasRoomDesign.Components.Pages
{
    public partial class RoomDesigner
    {
        [Inject] public IClientUIService _ClientUIService { get; set; }


        /*Parameters*/


        private double ToolBoxX = 200, ToolBoxY = 200; //Toolbox ekrandaki x,y koordinatları
        private bool isDraggingToolBox = false; //Toolbox seçili olup olmadığının kontrolu
        private double ToolBoxDragOffsetX = 0, ToolBoxDragOffsetY = 0; //Fare ile nesne seçildiğinde x,y koordinatlarının tutulması
        

        private double CurrentDesignItemX = 0, CurrentDesignItemY = 0;

        private double lastMouseX, lastMouseY;

        private double ScreenX = 0, ScreenY = 0;

        private bool isDraggingDesignItem = false;

        public string PrefixName = "A";
        public int PrefixNumber = 1;

        private bool hasDraggedItem = false;

        private string KeyDownListener = "";

        private DesignerState appState = new DesignerState();

        private List<DesignItem> ClipboardDesignItems = new List<DesignItem>();
        private List<DesignItem> RemoveItems = new List<DesignItem>();

        //Seçim alanı için kullanılacak değişkenler
        private bool isSelectingArea = false;
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
                DesignItemType.stage => (200, 75, "#000000", false),
                DesignItemType.door => (40, 75, "", false),
                _ => (40, 40, "#333333", false)
            };
            double defaultYPosition = 200;
            if (appState.Items.Any())
            {
                defaultYPosition = appState.GetLastItem()?.Y ?? 200;
            }

            double xPosition = 0;
            if (isDirectionToRight)
            {
                xPosition = appState.GetLastItem()?.X + 80 ?? 200;
            }
            else
            {
                xPosition = appState.GetLastItem()?.X - 80 ?? 200;
            }

            DesignItem newItem = new DesignItem
            {
                Name = PrefixName.ToUpper() + "-" + PrefixNumber,
                Type = itemType,
                X = xPosition,
                Y = defaultYPosition,
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
            DesignItemType.door=>(MarkupString)@"
                <svg width=""40"" height=""150"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
                    <path d=""M3 20h4""></path>
                    <path d=""M17 20h4""></path>
                    <path d=""M7 20V8""></path>
                    <path d=""M7 8a12 12 0 0 1 12 12"" stroke-dasharray=""2 2""></path>
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

            if (isDragGroupList)
            {
                GroupListX = e.ClientX - GroupListDragOffsetX;
                GroupListY = e.ClientY - GroupListDragOffsetY;
            }

            
            if (isSelectingArea)
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

            if (isDraggingDesignItem)
            {
                // 1. Izgara boyutumuzu belirliyoruz
                int snapSize = 20;

                // 2. Farenin "Bir önceki hareket ettiği noktadan" ne kadar uzaklaştığını bul
                double rawDeltaX = e.ClientX - lastMouseX;
                double rawDeltaY = e.ClientY - lastMouseY;

                // 3. İŞTE SİHİR BURADA: Fare 20'nin katı kadar (tam adım) ilerledi mi?
                // Örnek: Fare 35px gittiyse (35 / 20 = 1 tam adım). Fare 15px gittiyse (15 / 20 = 0 adım).
                int stepsX = (int)(rawDeltaX / snapSize);
                int stepsY = (int)(rawDeltaY / snapSize);

                // Eğer X veya Y ekseninde en az 1 tam adım (20px) atıldıysa harekete geç!
                if (stepsX != 0 || stepsY != 0)
                {
                    hasDraggedItem = true;

                    // Atılan tam adımı tekrar 20 ile çarpıp gerçek uygulanacak pikseli bul (Örn: 1 * 20 = 20px)
                    double snappedDeltaX = stepsX * snapSize;
                    double snappedDeltaY = stepsY * snapSize;

                    // Seçili orduyu hizalı bir şekilde kaydır!
                    foreach (var item in appState.SelectedItems)
                    {
                        item.X += snappedDeltaX;
                        item.Y += snappedDeltaY;
                    }

                    // 4. ÇOK KRİTİK: Farenin "Eski Konumunu" farenin ŞU ANKİ yeri yapmıyoruz!
                    // Sadece kullandığımız o 20px'lik kısmı ekliyoruz. 
                    // Böylece artan o 15 piksellik "küsurat" kaybolmuyor, bir sonraki harekette birikmeye devam ediyor!
                    lastMouseX += snappedDeltaX;
                    lastMouseY += snappedDeltaY;
                }
            }
            ScreenX = e.ClientX;
            ScreenY = e.ClientY;
        }

        private void OnMouseUpCanvas(MouseEventArgs e) //Canvas üzerinde mouse kontrolü bırakıldığında
        {
            isDraggingToolBox = false;

            isSelectingArea = false;

            isDraggingDesignItem = false;

            isDragGroupList = false;

        }


        

        private void OnCanvasMouseDown(MouseEventArgs e)
        {
            
            isSelectingArea = true;
            startX = e.ClientX;
            startY = e.ClientY;
            currentX = startX;
            currentY = startY;
            appState.ClearSelection();
        }


        private void DesignItemClick(MouseEventArgs e, DesignItem item) 
        {
            
            /*
            if (e.CtrlKey)
            {
                if (item.IsSelected)
                {
                    appState.DeSelectItem(item, notify: true);
                }
                else
                {
                    appState.SelectItem(item, notify: true);
                }
            }
            else
            {
                appState.ClearSelection();
                appState.SelectItem(item, notify: true);
            }
            */

        }

        private void DesignItemMouseDown(MouseEventArgs e, DesignItem item)
        {
            hasDraggedItem = false;

            // 2. KURAL: Eğer tıklanan koltuk ZATEN SEÇİLİ DEĞİLSE...
            if (!item.IsSelected)
            {
                // CTRL'ye basılmıyorsa eski seçimi temizle
                if (!e.CtrlKey)
                {
                    appState.ClearSelection();
                }
                // Ve sadece bu koltuğu seç
                appState.SelectItem(item, notify: true);
            }
            // DİKKAT: Eğer koltuk zaten seçiliyse hiçbir if() bloğuna girmez, seçimi KORUR!

            // 3. Sürükleme motorunu hazırla
            if (e.Button == 0) isDraggingDesignItem = true;
            lastMouseX = e.ClientX;
            lastMouseY = e.ClientY;

        }


        private void DesignItemMouseUp(MouseEventArgs e, DesignItem item)
        {
            //isDraggingDesignItem = false;
            // KURAL 3: Eğer fareyi bıraktık ama HİÇ SÜRÜKLEMEDİYSEK ve CTRL'ye basmıyorsak...
            // Demek ki kullanıcı sadece bu koltuğu "Tekli Seçmek" istedi!
            if (!hasDraggedItem && !e.CtrlKey)
            {
                appState.ClearSelection();
                appState.SelectItem(item, notify: true);
            }

            // İşlem bitti, ajanları uyut
            isDraggingDesignItem = false;
            hasDraggedItem = false;
        }

        private void CanvasAreaKeyboardDownListener(KeyboardEventArgs e) 
        {
            KeyDownListener = e.Key;
            if(e.CtrlKey && e.Key.ToLower() == "c")
            {
                ClipboardDesignItems.Clear();
                foreach (var item in appState.SelectedItems) 
                {
                    var newItem = item.Clone();
                    ClipboardDesignItems.Add(newItem);
                }
            }
            if(e.CtrlKey && e.Key.ToLower() == "v")
            {
                if (ClipboardDesignItems.Count() > 0)
                {
                    appState.ClearSelection();
                    foreach (var item in ClipboardDesignItems)
                    {
                        var newItem = item.Clone();
                        newItem.Name = PrefixName.ToUpper() + "-" + PrefixNumber;
                        newItem.Y += 80;
                        PrefixNumber++;
                        appState.AddItem(newItem);
                        appState.SelectItem(newItem, notify: false  );
                    }
                    foreach (var item in ClipboardDesignItems)
                    {
                        item.Y += 80;
                    }
                } 
            }
            if (e.Key.ToLower() == "delete")
            {
                if (appState.SelectedItems.Any())
                {
                    foreach (var item in appState.SelectedItems)
                    {
                        RemoveItems.Add(item);
                    }
                    appState.ClearSelection();
                    foreach(var item in RemoveItems)
                    {
                        appState.RemoveItem(item, notify:false );
                    }
                    RemoveItems = new List<DesignItem>();
                }
                
            }
        }


        //Groupda Renk Paleti Seçimi
        // Renk paletinin açık/kapalı durumu
        private bool isGroupColorPickerOpen = false;

        // Seçilen rengi tutacak değişken (Varsayılan olarak ilk renk)
        private GroupItem CreatadGroup = new GroupItem { Color = "#EF4444" };

        // 20 Adet Modern UI Rengi (Tailwind CSS referanslı, göz yormayan tonlar)
        private string[] groupColors = new string[]
        {
            "#EF4444", "#F97316", "#F59E0B", "#EAB308", "#84CC16",
            "#22C55E", "#10B981", "#14B8A6", "#06B6D4", "#0EA5E9",
            "#3B82F6", "#6366F1", "#8B5CF6", "#A855F7", "#D946EF",
            "#EC4899", "#F43F5E", "#64748B", "#71717A", "#18181B"
        };

        // Renk seçildiğinde çalışacak metot
        private void SelectGroupColor(string color)
        {
            CreatadGroup.Color = color;
            isGroupColorPickerOpen = false; // Seçim yapılınca paneli kapat
        }

        async Task CreateGroupButtonClick()
        {
            if (appState.SelectedItems.Count() == 0) return;
            if (String.IsNullOrEmpty(CreatadGroup.Name.Trim()))
            {
                await _ClientUIService.ShowError("Lütfen grup adı giriniz.");
            }
            else
            {
                appState.AddItemToGroup(CreatadGroup);
                appState.ClearSelection();
                appState.NotifyStateChanged();
                CreatadGroup = new GroupItem();
                await _ClientUIService.ShowSuccess("Grup başarıyla oluşturuldu.");
            }
                
        }

        private bool isDragGroupList = false;
        private double GroupListX = 50, GroupListY = 400;
        private double GroupListDragOffsetX = 0, GroupListDragOffsetY = 0;
        private void OnGroupListMouseDown(MouseEventArgs e)
        {
            isDragGroupList = true;
            GroupListDragOffsetX = e.ClientX - GroupListX;
            GroupListDragOffsetY = e.ClientY - GroupListY;
        }

        // Varsayılan yön: Sağ (To Right)
        private bool isDirectionToRight = true;

        // Tıklandığında yönü tersine çevirecek
        private void ToggleDirection()
        {
            isDirectionToRight = !isDirectionToRight;
        }

        private void ItemRotate(DesignItem item, string direction)
        {
            if (direction == "left")
            {
                if (item.Rotation == 0)
                {
                    item.Rotation = 345;
                }
                else
                {
                    item.Rotation = item.Rotation - 15;
                }
            }
            else
            {
                if (item.Rotation == 345)
                {
                    item.Rotation = 0;
                }
                else
                {
                    item.Rotation = item.Rotation + 15;
                }
            }
        }

    }
}
