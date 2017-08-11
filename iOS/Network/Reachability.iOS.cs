﻿// <copyright file="NetworkReachability.cs" company="Esri, Inc">
//      Copyright 2017 Esri.
//
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.
// </copyright>
// <author>Mara Stoica</author>

using System.Net;
using SystemConfiguration;
using CoreFoundation;

namespace IndoorRouting
{
    /// <summary>
    /// Network status.
    /// </summary>
    internal enum NetworkStatus
    {
        /// <summary>
        /// Network is not reacheable
        /// </summary>
        NotReachable,

        /// <summary>
        /// Network is reacheable 
        /// </summary>
        ReachableViaCarrierDataNetwork,

        /// <summary>
        /// Network is reacheable only via WiFi.
        /// </summary>
        ReachableViaWiFiNetwork
    }

    /// <summary>
    /// Reachability class helps determine if device is online. This will be different for every platform. 
    /// </summary>
    internal static partial class Reachability
    {
        /// <summary>
        /// The default route reachability.
        /// </summary>
        private static NetworkReachability defaultRouteReachability;

        /// <summary>
        /// Test if the network is available.
        /// </summary>
        /// <returns><c>true</c>, if network available was ised, <c>false</c> otherwise.</returns>
        private static bool IsNetworkAvailableImpl()
        {
            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            NetworkReachabilityFlags flags;

            return defaultRouteReachability.TryGetFlags(out flags) &&
            IsReachableWithoutRequiringConnection(flags);
        }

        /// <summary>
        /// Is the network reachable without requiring connection.
        /// </summary>
        /// <returns><c>true</c>, if reachable without requiring connection, <c>false</c> otherwise.</returns>
        /// <param name="flags">Network Flags.</param>
        private static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            var isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            var isConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
            {
                isConnectionRequired = true;
            }

            return isReachable && isConnectionRequired;
        }
    }
}
