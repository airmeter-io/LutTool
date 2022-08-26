using System.Collections.Generic;
using System.IO;
using System.Linq;
using LutLib.Model;

namespace LutLib.Controllers
{
    public class Ssd16xxController : ControllerType
    {
        public Ssd16xxController(int pExpectedLength, int pNumGroups, bool pIncludeFrameRates, bool pIncludeGroups)
        {
            LutLength = pExpectedLength;
            NumGroups = pNumGroups;
            HasFrameRates = pIncludeFrameRates;
            HasPhaseGroups = pIncludeGroups;
        }

        public override LutGroup[] ParseValues(byte[] pLutData)
        {
            if (pLutData.Length != LutLength)
                throw new InvalidDataException($"The {ToString()} uses a {LutLength} byte lut this lut is {pLutData.Length} bytes");

            var groups = new List<LutGroup>();
            for(var i = 0; i < NumGroups; i++)
                groups.Add(new LutGroup());
            

            var index = 0;
            for (var lutOffset = 0; lutOffset < LutPhaseInfo.NumLuts; lutOffset++)
            {
                foreach (var group in groups)
                {
                    var b = pLutData[index];
                    index++;

                    var phaseD = b & 0x3;
                    b >>= 2;
                    var phaseC = b & 0x3;
                    b >>= 2;
                    var phaseB = b & 0x3;
                    b >>= 2;
                    var phaseA = b & 0x3;
                    group.Phases[LutPhaseType.PhaseA].Sources[lutOffset] = (VoltageSource)phaseA;
                    group.Phases[LutPhaseType.PhaseB].Sources[lutOffset] = (VoltageSource)phaseB;
                    group.Phases[LutPhaseType.PhaseC].Sources[lutOffset] = (VoltageSource)phaseC;
                    group.Phases[LutPhaseType.PhaseD].Sources[lutOffset] = (VoltageSource)phaseD;
                }
            }

            foreach (var group in groups)
            {
                group.Phases[LutPhaseType.PhaseA].PhaseLength = pLutData[index];
                index++;
                group.Phases[LutPhaseType.PhaseB].PhaseLength = pLutData[index];
                index++;

                if (HasPhaseGroups)
                {
                    group.PhaseGroups[(LutPhaseType.PhaseA, LutPhaseType.PhaseB)].StateRepeatCountingNumber =
                        pLutData[index];
                    index++;
                }

                group.Phases[LutPhaseType.PhaseC].PhaseLength = pLutData[index];
                index++;
                group.Phases[LutPhaseType.PhaseD].PhaseLength = pLutData[index];
                index++;

                if (HasPhaseGroups)
                {
                    group.PhaseGroups[(LutPhaseType.PhaseC, LutPhaseType.PhaseD)].StateRepeatCountingNumber =
                        pLutData[index];
                    index++;
                }

                group.RepeatCountingNumber = pLutData[index];
                index++;
            }


            if (HasFrameRates)
            {
                for (var baseIndex = 0; baseIndex <= groups.Count-2; baseIndex += 2)
                {
                    var b = pLutData[index];
                    index++;
                    var groupSubset = groups.Skip(baseIndex).Take(2).Reverse();
                    foreach (var group in groupSubset)
                    {
                        group.FrameRate = (uint)(b & 0xf);
                        b >>= 4;
                    }
                }
            }

            if (HasPhaseGroups)
            {
                for (var baseIndex = 0; baseIndex <= 8; baseIndex += 4)
                {
                    var b = pLutData[index];
                    index++;
                    var groupSubset = groups.Skip(baseIndex).Take(4).Reverse();
                    foreach (var group in groupSubset)
                    {
                        group.PhaseGroups[(LutPhaseType.PhaseC, LutPhaseType.PhaseD)].Xon = (b & 1) != 0;
                        group.PhaseGroups[(LutPhaseType.PhaseA, LutPhaseType.PhaseB)].Xon = (b & 2) != 0;
                        b >>= 2;
                    }
                }
            }


            return groups.ToArray();
        }

        public override byte[] CompileToLut(LutGroup[] pGroups)
        {
            var result = new byte[LutLength];
            var index = 0;
            for (var lutOffset = 0; lutOffset < LutPhaseInfo.NumLuts; lutOffset++)
            {
                foreach (var group in pGroups)
                {
                    byte b = (byte)group.Phases[LutPhaseType.PhaseA].Sources[lutOffset];
                    b <<= 2;
                    b |= (byte)group.Phases[LutPhaseType.PhaseB].Sources[lutOffset];
                    b <<= 2;
                    b |= (byte)group.Phases[LutPhaseType.PhaseC].Sources[lutOffset];
                    b <<= 2;
                    b |= (byte)group.Phases[LutPhaseType.PhaseD].Sources[lutOffset];
                    result[index] = b;
                    index++;
                }
            }

            foreach (var group in pGroups)
            {
                result[index] = (byte)group.Phases[LutPhaseType.PhaseA].PhaseLength;
                index++;
                result[index] = (byte)group.Phases[LutPhaseType.PhaseB].PhaseLength;
                index++;


                if (HasPhaseGroups)
                {
                    result[index] = (byte)group.PhaseGroups[(LutPhaseType.PhaseA, LutPhaseType.PhaseB)]
                        .StateRepeatCountingNumber;
                    index++;
                }

                result[index] = (byte)group.Phases[LutPhaseType.PhaseC].PhaseLength;
                index++;
                result[index] = (byte)group.Phases[LutPhaseType.PhaseD].PhaseLength;
                index++;

                if (HasPhaseGroups)
                {
                    result[index] = (byte)group.PhaseGroups[(LutPhaseType.PhaseC, LutPhaseType.PhaseD)]
                        .StateRepeatCountingNumber;
                    index++;
                }

                result[index] = (byte)group.RepeatCountingNumber;
                index++;
            }

            if (HasFrameRates)
            {
                for (var baseIndex = 0; baseIndex <= pGroups.Length - 2; baseIndex += 2)
                {
                    byte b = 0;
                    var groupSubset = pGroups.Skip(baseIndex).Take(2);
                    foreach (var group in groupSubset)
                    {
                        b <<= 4;
                        b |= (byte)group.FrameRate;


                    }

                    result[index] = b;
                    index++;
                }
            }

            if (HasPhaseGroups)
            {
                for (var baseIndex = 0; baseIndex <= 8; baseIndex += 4)
                {
                    byte b = 0;

                    var groupSubset = pGroups.Skip(baseIndex).Take(4);
                    foreach (var group in groupSubset)
                    {
                        b <<= 2;
                        if (group.PhaseGroups[(LutPhaseType.PhaseC, LutPhaseType.PhaseD)].Xon) b |= 1;
                        if (group.PhaseGroups[(LutPhaseType.PhaseA, LutPhaseType.PhaseB)].Xon) b |= 2;

                    }

                    result[index] = b;
                    index++;
                }
            }

            return result;
        }

        public override int LutLength { get; }

       

        public override bool HasPhaseGroups { get; }
        public override bool HasFrameRates { get; }

        public int NumGroups { get; }
    }
}