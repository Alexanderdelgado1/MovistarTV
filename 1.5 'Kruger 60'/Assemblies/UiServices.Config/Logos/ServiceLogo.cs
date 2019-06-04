// Copyright (C) 2014-2019, GitHub/Codeplex user AlphaCentaury
// 
// All rights reserved, except those granted by the governing license of this software.
// See 'license.txt' file in the project root for complete license information.
// 
// http://www.alphacentaury.org/movistartv https://github.com/AlphaCentaury

using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace IpTviewr.UiServices.Configuration.Logos
{
    public class ServiceLogo : BaseLogo
    {
        internal ServiceLogo(ILogoMapping mapping, string mappingKey, string entry, string uniqueKey) : base(mapping, mappingKey, entry, uniqueKey)
        {
            // no-op
        } // constructor

        protected override string ImageNotFoundExceptionText => Properties.Texts.ExceptionLogosServiceImageNotFound;

        protected override string ImageLoadExceptionText => Properties.Texts.ExceptionLogosServiceImageLoadError;
    } // class ServiceLogo
} // namespace
