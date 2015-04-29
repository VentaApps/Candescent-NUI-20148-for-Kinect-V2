using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.HandTracking.Mouse;

namespace CCT.NUI.MouseControl
{
    public class ModeCombination
    {
        public ModeCombination(CursorMode cursorMode, ClickMode clickMode)
        {
            this.CursorMode = cursorMode;
            this.ClickMode = clickMode;
        }

        public ClickMode ClickMode { get; set; }

        public CursorMode CursorMode { get; set; }

        public override string ToString()
        {
            return "Cursor: " + this.CursorMode.ToString() + " Click: " + ClickMode.ToString();
        }

        public static IEnumerable<ModeCombination> ValidCombinations
        {
            get
            {
                yield return new ModeCombination(CursorMode.Finger, ClickMode.TwoFinger);
                yield return new ModeCombination(CursorMode.Finger, ClickMode.SecondHand);

                yield return new ModeCombination(CursorMode.CenterOfCluster, ClickMode.TwoFinger);
                yield return new ModeCombination(CursorMode.CenterOfCluster, ClickMode.SecondHand);
                yield return new ModeCombination(CursorMode.CenterOfCluster, ClickMode.Hand);

                yield return new ModeCombination(CursorMode.CenterOfHand, ClickMode.TwoFinger);
                yield return new ModeCombination(CursorMode.CenterOfHand, ClickMode.SecondHand);
                yield return new ModeCombination(CursorMode.CenterOfHand, ClickMode.Hand);

                yield return new ModeCombination(CursorMode.HandTracking, ClickMode.TwoFinger);
                yield return new ModeCombination(CursorMode.HandTracking, ClickMode.Hand);
            }
        }
    }
}
