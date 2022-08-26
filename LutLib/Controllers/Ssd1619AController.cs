namespace LutLib.Controllers
{
    public class Ssd1619AController : Ssd16xxController
    {
        public Ssd1619AController() : base(70, 7, false, false)
        {

        }

        public override string ToString() => "SSD1619A";
    }
}