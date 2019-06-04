// Copyright (C) 2014-2019, GitHub/Codeplex user AlphaCentaury
// 
// All rights reserved, except those granted by the governing license of this software.
// See 'license.txt' file in the project root for complete license information.
// 
// http://www.alphacentaury.org/movistartv https://github.com/AlphaCentaury

using IpTviewr.UiServices.Configuration.Schema2014.Logos;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace IpTviewr.UiServices.Configuration.Logos
{
    public partial class ServiceLogoMappings : ILogoMapping
    {
        private IDictionary<string, ReplacementDomain> _domainMappings;
        private Dictionary<string, ServiceDomainMapping> _serviceMappings;
        private IDictionary<string, LogosCollection> _collections;

        public string BasePathLogos
        {
            get;
            private set;
        } // BasePathServiceLogos

        public ServiceLogoMappings(DomainMappingsXml domainMappings, ServiceMappingsXml serviceMappings, string logosPath)
        {
            Init(domainMappings, serviceMappings, logosPath);
        } // constructor

        public ServiceLogoMappings(string domainMappingsXmlFilename, string serviceMappingsXmlFile, string logosPath)
        {
            var domainMappings = LogosCommon.ParseDomainMappingsXml(domainMappingsXmlFilename);
            var serviceMappings = LogosCommon.ParseServiceMappingsXml(serviceMappingsXmlFile);

            Init(domainMappings, serviceMappings, logosPath);
        } // constructor

        public ServiceLogo Get(string serviceDomainName, string providerDomain, string serviceName, string serviceTypeId)
        {
            while (true)
            {
                if (providerDomain == null) throw new ArgumentNullException(nameof(providerDomain));
                if (serviceDomainName == null) serviceDomainName = providerDomain;
                if (serviceName == null) serviceName = Properties.InvariantTexts.ServiceNameAny;

                serviceDomainName = serviceDomainName.ToLowerInvariant();
                serviceName = serviceName.ToLowerInvariant();

                var firstReplacementChance = true;
                while (serviceDomainName != null)
                {
                    // replace domain?
                    if (_domainMappings.TryGetValue(serviceDomainName, out var replacement))
                    {
                        if ((replacement.IsMandatory) || (!firstReplacementChance))
                        {
                            serviceDomainName = replacement.Replacement;
                            firstReplacementChance = true;
                            continue;
                        } // if
                    } // if

                    if (!firstReplacementChance)
                    {
                        serviceDomainName = GetParentDomain(serviceDomainName);
                        continue;
                    } // if

                    // locate service logo for given domain
                    if (!_serviceMappings.TryGetValue(serviceDomainName, out var serviceLogos))
                    {
                        firstReplacementChance = false;
                        continue;
                    } // if

                    if (!serviceLogos.Logos.TryGetValue(serviceName, out var logoFile))
                    {
                        if (!serviceLogos.Logos.TryGetValue(Properties.InvariantTexts.ServiceNameAny, out logoFile))
                        {
                            firstReplacementChance = false;
                            continue;
                        } // if
                    } // if

                    var domain = serviceLogos.DomainRedirection ?? serviceDomainName;
                    return GetLogo(domain, logoFile);
                } // while

                // avoid infinite recursion if default domain name contains no logos or doesn't exists
                if (providerDomain == Properties.InvariantTexts.DefaultDomainNameServiceLogo)
                {
                    return GetLogo(null, null);
                } // if

                // obtain default logo
                serviceDomainName = null;
                providerDomain = Properties.InvariantTexts.DefaultDomainNameServiceLogo;
                serviceName = serviceTypeId;
            } // while
        } // Get

        private ServiceLogo GetLogo(string domain, string logoFile)
        {
            if ((domain == null) || (logoFile == null))
            {
                return new ServiceLogo(this, "", "", @"/services/<unknown>");
            } // if

            var collection = _collections[domain];
            if (logoFile.StartsWith("hd_"))
            {
                logoFile = $@"HD/{logoFile.Substring(3)}";
            } // if

            return new ServiceLogo(this, domain, logoFile, $@"/services/{collection.Name}/{domain}/{logoFile}");
        } // GetLogo

        public ServiceLogo FromServiceKey(string serviceKey)
        {
            var pos = serviceKey.IndexOf('@');
            if (pos < 1) throw new ArgumentException();
            if ((pos + 1) == serviceKey.Length) throw new ArgumentException();

            var service = serviceKey.Substring(0, pos);
            var domain = serviceKey.Substring(pos + 1);

            return Get(null, domain, service, null);
        } // FromServiceKey

        private static string GetParentDomain(string domainName)
        {
            var parts = domainName.Split('.');
            return parts.Length <= 2 ? null : string.Join(".", parts, 1, parts.Length - 1);
        } // GetParentDomain

        private void Init(DomainMappingsXml domainMappings, ServiceMappingsXml serviceMappings, string logosPath)
        {
            BasePathLogos = logosPath;
            BuildMapping(domainMappings);
            BuildMapping(serviceMappings);
        } // Init

        private void BuildMapping(DomainMappingsXml mapping)
        {
            var q = from package in mapping.Collections
                    from mp in package.Mappings
                    select 0;
            var count = q.Count();

            var entries = from collection in mapping.Collections
                          from mp in collection.Mappings
                          select new { Collection = collection, Mapping = mp };

            _domainMappings = new Dictionary<string, ReplacementDomain>(count, StringComparer.OrdinalIgnoreCase);
            foreach (var entry in entries)
            {
                try
                {
                    _domainMappings.Add(entry.Mapping.DomainName, new ReplacementDomain
                    {
                        IsMandatory = entry.Mapping.Mandatory,
                        Replacement = entry.Mapping.ReplacementDomain,
                    });
                }
                catch (ArgumentException ex) // duplicated key (domain name)
                {
                    throw new ApplicationException(
                        string.Format(Properties.Texts.ExceptionLogosDomainMappingsDuplicatedDomain, entry.Mapping.DomainName), ex);
                } // try-catch
            } // foreach
        } // BuildMapping

        protected void BuildMapping(ServiceMappingsXml mapping)
        {
            var q = from collection in mapping.Collections
                    from domain in collection.Domains
                    select 0;
            var count = q.Count();

            _serviceMappings = new Dictionary<string, ServiceDomainMapping>(count, StringComparer.OrdinalIgnoreCase);
            _collections = new Dictionary<string, LogosCollection>(count, StringComparer.OrdinalIgnoreCase);

            var entries = from collection in mapping.Collections
                          from domain in collection.Domains
                          select new { Collection = collection, Domain = domain };

            foreach (var entry in entries)
            {
                var domainMappings = new ServiceDomainMapping()
                {
                    DomainRedirection = entry.Domain.RedirectDomainName,
                    Logos = new Dictionary<string, string>(entry.Domain.Mappings.Length, StringComparer.OrdinalIgnoreCase),
                };
                try
                {
                    _serviceMappings.Add(entry.Domain.DomainName, domainMappings);
                    _collections.Add(entry.Domain.DomainName, new LogosCollection(name: entry.Collection.Name,
                        package: entry.Collection.Package,
                        archivePath: Path.Combine(BasePathLogos, entry.Collection.Package) + ".zip",
                        key: $@"/service/{entry.Collection.Package}"));
                }
                catch (ArgumentException ex) // duplicated key (domain name)
                {
                    throw new ApplicationException(
                        string.Format(Properties.Texts.ExceptionLogosServiceMappingsDuplicatedDomain,
                        entry.Domain.DomainName), ex);
                } // try-catch

                foreach (var mp in entry.Domain.Mappings)
                {
                    try
                    {
                        domainMappings.Logos.Add(mp.Name, mp.Logo);
                    }
                    catch (ArgumentException ex) // duplicated key (domain service name)
                    {
                        throw new ApplicationException(
                            string.Format(Properties.Texts.ExceptionLogosServiceMappingsDuplicatedService,
                            mp.Name, entry.Domain.DomainName), ex);
                    } // try-catch
                } // foreach mp
            } // foreach domain
        } // BuildMapping

        #region ILogoMapping implementation

        Stream ILogoMapping.GetImage(string key, string entry, LogoSize size)
        {
            var quality = GetQuality(entry, out entry);
            return GetZipEntry(key, entry, quality, ((int) size).ToString(), ".png", out _)?.Open();
        } // ILogoMapping.GetImage

        ZipArchiveEntry ILogoMapping.GetIcon(string key, string entry, out DateTime lastModifiedUtc)
        {
            var quality = GetQuality(entry, out entry);
            return GetZipEntry(key, entry, quality, entry, ".ico", out lastModifiedUtc);
        } // ILogoMapping.GetImage

        private ZipArchiveEntry GetZipEntry(string key, string entry, string quality, string zipFile, string extension, out DateTime lastModifiedUtc)
        {
            lastModifiedUtc = DateTime.MinValue;
            if (!_collections.TryGetValue(key, out var collection)) return null;

            var zip = BaseLogo.GetZipArchive(collection.ArchivePath);
            lastModifiedUtc = collection.GetLastModifiedUtc();

            while (quality != null)
            {
                var entryName = $@"{key}{Path.DirectorySeparatorChar}{entry}{Path.DirectorySeparatorChar}{quality}{Path.DirectorySeparatorChar}{zipFile}{extension}";
                var zipEntry = BaseLogo.GetZipEntry(zip, entryName);
                if (zipEntry != null) return zipEntry;

                quality = (quality == "(default)") ? null : "(default)";
            } // while

            return null;
        } // GetZipEntry

        private static string GetQuality(string entry, out string zipFile)
        {
            int pos;

            if ((pos = entry.IndexOf('/')) >= 0)
            {
                zipFile = entry.Substring(pos + 1);
                return entry.Substring(0, pos);
            } // if

            zipFile = entry;
            return @"(default)";
        } // GetQuality

        #endregion
    } // class ServiceLogoMappings
} // namespace
