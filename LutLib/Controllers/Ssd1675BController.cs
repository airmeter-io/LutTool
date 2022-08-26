namespace LutLib.Controllers
{
    public class Ssd1675BController : Ssd16xxController
    {
        public Ssd1675BController() : base(100, 10, false, false)
        {

        }

        public override string ToString() => "SSD1675B";
    }
}