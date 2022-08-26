namespace LutLib.Model
{
    public class LutPhaseInfo
    {
        public uint PhaseLength { get; set; }
        public VoltageSource[] Sources = new VoltageSource[NumLuts];

        public const int NumLuts = 5;
    }
}