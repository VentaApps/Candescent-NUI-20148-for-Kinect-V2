using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCT.NUI.Tests
{
    public static class AssertAsync
    {
        public static void IsTrue(Func<bool> assertion, int timeoutInMilliSeconds)
        {
            if (!SpinWait.SpinUntil(assertion, timeoutInMilliSeconds))
            {
                throw new AssertFailedException("Timeout");
            }
        }

        public static void AreEqual(object expected, object actual, int timeoutInMilliSeconds)
        {
            IsTrue(() => object.Equals(expected, actual), timeoutInMilliSeconds);
        }
    }
}
