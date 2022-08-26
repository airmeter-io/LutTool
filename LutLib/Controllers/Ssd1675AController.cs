namespace LutLib.Controllers
{
    public class Ssd1675AController : Ssd16xxController
    {
        public Ssd1675AController() : base(70, 7, false, false)
        {

        }

        public override string ToString() => "SSD1675A";
    }
}