using System.Collections.ObjectModel;
using LutLib.Model;
using LutTool.View;
namespace LutLib.View
{
    public class LutPhaseView : BaseView
    {
        private readonly LutPhaseInfo _phaseInfo;
        public LutPhaseType Type { get; }

        public LutPhaseView(LutGroupView pGroup, LutPhaseType pType, LutPhaseInfo pPhaseInfo)
        {
            _phaseInfo = pPhaseInfo;
            Type = pType;
            int index = 0;
            foreach (var source in pPhaseInfo.Sources)
            {
                var indexCap = index;
                VoltageSources.Add(new VoltageSourceView(index++, () => pPhaseInfo.Sources[indexCap], pSource =>
                {
                    System.Console.WriteLine($"Group: {pGroup.Index}  Phase: {pType} Source ({indexCap}): {pSource}");
                    pPhaseInfo.Sources[indexCap] = pSource;
                }));
            }
        }

        public uint PhaseLength
        {
            get => _phaseInfo.PhaseLength;
            set
            {
                _phaseInfo.PhaseLength = value;
                OnPropertyChanged(nameof(PhaseLength));
            }
        }

        public ObservableCollection<VoltageSourceView> VoltageSources { get; } = new();
    }
}