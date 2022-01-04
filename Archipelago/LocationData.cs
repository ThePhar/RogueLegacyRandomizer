namespace Archipelago
{
    public class LocationData
    {
        public LocationData(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public int Code { get; private set; }
        public string Name { get; private set; }
    }
}