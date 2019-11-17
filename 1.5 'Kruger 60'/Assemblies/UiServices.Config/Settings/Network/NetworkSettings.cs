// Copyright (C) 2014-2019, GitHub/Codeplex user AlphaCentaury
// 
// All rights reserved, except those granted by the governing license of this software.
// See 'license.txt' file in the project root for complete license information.
// 
// http://www.alphacentaury.org/movistartv https://github.com/AlphaCentaury

using IpTviewr.UiServices.Configuration.Schema2014.Config;
using System;
using System.Xml.Serialization;

namespace IpTviewr.UiServices.Configuration.Settings.Network
{
    [Serializable]
    [XmlRoot("Network", Namespace = ConfigCommon.ConfigXmlNamespace)]
    public class NetworkSettings : IConfigurationItem
    {
        public static NetworkSettings GetDefaultSettings()
        {
            var result = new NetworkSettings()
            {
                MulticastProxy = MulticastProxy.GetDefaultSettings()
            };

            return result;
        } // GetDefaultSettings

        public MulticastProxy MulticastProxy
        {
            get;
            set;
        } // MulticastProxy

        #region IConfigurationItem implementation

        bool IConfigurationItem.SupportsInitialization => false;

        bool IConfigurationItem.SupportsValidation => false;

        InitializationResult IConfigurationItem.Initialize()
        {
            throw new NotSupportedException();
        } // IConfigurationItem.Initialize

        string IConfigurationItem.Validate(string ownerTag)
        {
            throw new NotSupportedException();
        } // IConfigurationItem.Validate

        #endregion
    } // class NetworkSettings
} // namespace
