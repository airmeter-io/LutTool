namespace LutLib.Controllers
{
    public class Ssd1681Controller : Ssd16xxController
    {
        public Ssd1681Controller() : base(153, 12, true, true)
        {

        }

        public override string ToString() => "SSD1681";
    }
}