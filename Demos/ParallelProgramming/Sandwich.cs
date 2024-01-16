namespace ConsoleApp1
{
    public class Sandwich
    {
        public string Type { get; set; }

        public string Sauce { get; set; }

        public string Extra { get; set; }

        public override string ToString()
        {
            return $"{this.Type} sandwich with {this.Sauce} sauce and {this.Extra}"; ;
        }
    }
}
