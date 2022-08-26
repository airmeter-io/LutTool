namespace LutLib.Controllers
{
    public class Ssd1677Controller : Ssd16xxController
    {
        public Ssd1677Controller() : base(105, 10, true, false)
        {

        }

        public override string ToString() => "SSD1677";
    }
}