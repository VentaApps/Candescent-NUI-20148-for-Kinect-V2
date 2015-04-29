using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking.Mouse
{
    public class TwoFingerClickMode : ClickModeBase
    {
        private bool mouseDown;
        private DateTime? twoFingersDetected;

        public override void Process(HandCollection handData)
        {
            var fingerCount = handData.Hands[0].FingerCount;
            if (twoFingersDetected == null && fingerCount == 2)
            {
                twoFingersDetected = DateTime.Now;
            }
            if (fingerCount == 1)
            {
                twoFingersDetected = null;
            }

            if (twoFingersDetected.HasValue && DateTime.Now > twoFingersDetected.Value.AddMilliseconds(100))
            {
                if (fingerCount == 2 && !this.mouseDown)
                {
                    UserInput.MouseDown();
                    this.mouseDown = true;
                }
            }
            if (fingerCount == 1 && this.mouseDown)
            {
                UserInput.MouseUp();
                this.mouseDown = false;
            }
        }

        public override ClickMode EnumValue
        {
            get { return ClickMode.TwoFinger; }
        }
    }
}
