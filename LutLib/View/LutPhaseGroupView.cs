using LutLib.Model;

namespace LutLib.View
{
    public class LutPhaseGroupView : BaseView
    {
        private readonly LutPhaseGroup _info;
        public (LutPhaseType, LutPhaseType) GroupType { get; }

        public LutPhaseGroupView((LutPhaseType, LutPhaseType) pGroupType, LutPhaseGroup pInfo)
        {
            _info = pInfo;
            GroupType = pGroupType;
            var (left, right) = pGroupType;
            Left = left;
            Right = right;
        }

        public LutPhaseType Right { get; set; }

        public LutPhaseType Left { get; set; }

        public uint StateRepeatCountingNumber
        {
            get => _info.StateRepeatCountingNumber;
            set
            {
                _info.StateRepeatCountingNumber = value;
                OnPropertyChanged(nameof(StateRepeatCountingNumber));
            }
        }

        public bool Xon
        {
            get => _info.Xon;
            set
            {
                _info.Xon = value;
                OnPropertyChanged(nameof(Xon));
            }
        }
    }
}