using System.Collections.Generic;

namespace LutLib.Model
{
    public class LutGroup
    {
        public Dictionary<LutPhaseType, LutPhaseInfo> Phases { get; } = new()
        {
            {
                LutPhaseType.PhaseA, 
                new LutPhaseInfo()
            },
            {
                LutPhaseType.PhaseB,
                new LutPhaseInfo()
            },
            {
                LutPhaseType.PhaseC,
                new LutPhaseInfo()
            },
            {
                LutPhaseType.PhaseD,
                new LutPhaseInfo()
            }
        };
        public uint RepeatCountingNumber { get; set; }

        public Dictionary<(LutPhaseType, LutPhaseType), LutPhaseGroup> PhaseGroups = new()
        {
            {
                (LutPhaseType.PhaseA, LutPhaseType.PhaseB),
                new LutPhaseGroup()
            },
            {
                (LutPhaseType.PhaseC, LutPhaseType.PhaseD),
                new LutPhaseGroup()
            }
        };
        
        public uint FrameRate { get; set; }
        

    }
}