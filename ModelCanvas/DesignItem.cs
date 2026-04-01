namespace CanvasRoomDesign.ModelCanvas
{
    public class DesignItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DesignItemType Type { get; set; }

        public string Name { get; set; } = "";

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; } = 40;
        public double Height { get; set; } = 40;
        public double Rotation { get; set; } = 0;
        public string Color { get; set; } = "#333333";
        public bool IsSelected { get; set; }

        public bool IsSellable { get; set; } = false;

        public bool IsAddedToGroup { get; set; } = false;
        public Guid? GroupId { get; set; }
        public string GroupColor { get; set; } = "";
        public string GroupName { get; set; } = "";


        public DesignItem Clone()
        {
            return new DesignItem
            {
                Type = this.Type,
                Name = this.Name,
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Rotation = this.Rotation,
                Color = this.Color,
                IsSelected = false,
                IsSellable = this.IsSellable,
                IsAddedToGroup = this.IsAddedToGroup,
                GroupId = this.GroupId,
                GroupColor = this.GroupColor,
                GroupName = this.GroupName
            };
        }


    }
}
