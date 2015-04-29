using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking.Mouse
{
    public class HandClickMode : ClickModeBase
    {
        private bool mouseDown;

        public override void Process(HandCollection handData)
        {
            var fingerCount = handData.Hands[0].FingerCount;

            if (fingerCount <= 1 && !this.mouseDown)
            {
                UserInput.MouseDown();
                this.mouseDown = true;
            }

            if (fingerCount >= 2 && this.mouseDown)
            {
                UserInput.MouseUp();
                this.mouseDown = false;
            }
        }

        public override ClickMode EnumValue
        {
            get { return ClickMode.Hand; }
        }
    }
}
