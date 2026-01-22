namespace DCD
{
    public class Stat
    {
        public string name { get; set; } = "";
        public float value { get; set; } = 0f;
        public Stat(string name, float value)
        {
            this.name = name;
            this.value = value;
        }
    }
    
}