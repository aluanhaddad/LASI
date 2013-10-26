﻿using LASI;
using LASI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LASI.Core.Interop;

namespace LASI.InteropLayer
{
    /// <summary>
    /// Controls global performance and resource usage settings.
    /// </summary>
    public static class PerformanceManager
    {
        /// <summary>
        /// Sets the overall performance level to the provided value.
        /// </summary>
        /// <param name="mode">The PerformanceLevel value indicating the new performance and resource usage settings to adobt.</param>
        public static void SetPerformanceLevel(PerforamanceLevel mode) {
            switch (mode) {
                case PerforamanceLevel.High:
                    Concurrency.SetFromResourceMode(ResourceMode.High);
                    Memory.SetFromResourceMode(ResourceMode.High);
                    break;
                case PerforamanceLevel.Normal:
                    Concurrency.SetFromResourceMode(ResourceMode.Normal);
                    Memory.SetFromResourceMode(ResourceMode.Normal);
                    break;
                case PerforamanceLevel.Low:
                    Concurrency.SetFromResourceMode(ResourceMode.Low);
                    Memory.SetFromResourceMode(ResourceMode.Low);
                    break;
            }
        }
    }
    /// <summary>
    /// The Performance Levels the application may operate under.
    /// </summary>
    public enum PerforamanceLevel
    {
        /// <summary>
        /// High Performance.
        /// </summary>
        High,
        /// <summary>
        /// Normal Performance.
        /// </summary>
        Normal,
        /// <summary>
        /// Low Performance.
        /// </summary>
        Low,
    }
}
