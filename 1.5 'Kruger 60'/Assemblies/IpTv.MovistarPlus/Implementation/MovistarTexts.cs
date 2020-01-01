// ==============================================================================
// 
//   Copyright (C) 2014-2020, GitHub/Codeplex user AlphaCentaury
//   All rights reserved.
// 
//     See 'LICENSE.MD' file (or 'license.txt' if missing) in the project root
//     for complete license information.
// 
//   http://www.alphacentaury.org/movistartv
//   https://github.com/AlphaCentaury
// 
// ==============================================================================

using IpTviewr.IpTvServices.Implementation;

namespace IpTviewr.IpTvServices.MovistarPlus.Implementation
{
    public sealed class MovistarTexts: ServiceTexts
    {
        protected override ITvServiceProviderTexts GetProviderTexts() => new MovistarProviderTexts();
    } // class MovistarTexts
} // namespace
