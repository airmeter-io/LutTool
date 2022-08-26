namespace LutLib.Controllers
{
    public class Ssd1680Controller : Ssd16xxController
    {
        public Ssd1680Controller() : base(153, 12, true, true)
        {

        }

        public override string ToString() => "SSD1680";
    }
}