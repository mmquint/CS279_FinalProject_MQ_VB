﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    public class HelperFunctions
    {

        internal static string GetTimeStamp(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmssffff");
        }

        internal static int GetMSeconds(DateTime dateTime)
        {
            return (int) dateTime.TimeOfDay.TotalMilliseconds;
        }

        internal static string GetFolderName(DateTime dateTime)
        {
            string timeStamp = GetTimeStamp(dateTime);
            return "session" + timeStamp;
        }
    }
}
    