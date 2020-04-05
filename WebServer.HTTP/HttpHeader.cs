namespace WebServer.HTTP
{
    public class HttpHeader
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name} {Value}";
        }
    }
}