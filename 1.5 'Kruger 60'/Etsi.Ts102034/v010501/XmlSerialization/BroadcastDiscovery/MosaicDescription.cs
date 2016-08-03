﻿// Copyright (C) 2014-2016, Codeplex/GitHub user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Etsi.Ts102034.v010501.XmlSerialization.BroadcastDiscovery
{
    [GeneratedCode("myxsdtool", "0.0.0.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "urn:dvb:metadata:iptv:sdns:2012-1")]
    public partial class MosaicDescription
    {
        [XmlElement("LogicalCell")]
        public MosaicDescriptionLogicalCell[] LogicalCell;

        [XmlAttribute]
        [DefaultValue(true)]
        public bool EntryPoint;

        [XmlAttribute]
        public string HorizontalCellsNumber;

        [XmlAttribute]
        public string VerticalCellsNumber;

        public MosaicDescription()
        {
            this.EntryPoint = true;
        } // default constructor
    } // class MosaicDescription
} // namespace
