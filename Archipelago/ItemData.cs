namespace Archipelago
{
    public class ItemData
    {
        public ItemData(int code, string name, ItemType type)
        {
            Code = code;
            Name = name;
            Type = type;
        }

        public ItemType Type { get; private set; }
        public int Code { get; private set; }
        public string Name { get; private set; }
    }
}