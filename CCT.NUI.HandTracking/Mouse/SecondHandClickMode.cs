using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking.Mouse
{
    public class SecondHandClickMode : ClickModeBase
    {
        private bool mouseDown;

        public override void Process(HandCollection handData)
        {
            if (!mouseDown && handData.Count >= 2)
            {
                this.MouseDown();
                this.mouseDown = true;
            }
            if (mouseDown && handData.Count < 2)
            {
                this.MouseUp();
                this.mouseDown = false;
            }
        }

        public override ClickMode EnumValue
        {
            get { return ClickMode.SecondHand; }
        }
    }
}
