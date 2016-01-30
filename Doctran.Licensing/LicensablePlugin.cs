// <copyright file="LicensablePlugin.cs" company="Christopher Pardi">
//     Copyright (c) 2015 Christopher Pardi. All rights reserved.
// </copyright>

namespace Doctran.Licensing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Helper;
    using Parsing;
    using ParsingElements.FortranObjects;
    using Plugins;
    using Reporting;
    using global::Licensing;

    public abstract class LicensablePlugin : IPlugin
    {
        private readonly License _lic;

        protected LicensablePlugin()
        {
            _lic = LicenseProvider.GetLicense(this.GetLicensePath(), this.GetSecretKey());
        }

        public bool AmILicensed
        {
            get
            {
                var validLic = false;
                try
                {
                    var lic = LicenseProvider.GetLicense(this.GetLicensePath(), this.GetSecretKey());
                    if (lic.Activated)
                    {
                        validLic = LicenseVerifier.IsValid(lic);
                        if (!validLic)
                        {
                            Report.Error(
                                pub => pub.DescriptionReason(ReportGenre.Licensing, $"License for '{this.FriendlyName}' was found but could not be validated."), new Exception("License found but could not be validated."));
                        }
                    }
                }
                catch (ApplicationException e)
                {
                    Report.Error(pub => pub.DescriptionReason(ReportGenre.Licensing, $"Licensing error in '{this.FriendlyName}'. {e.Message}"), e);
                }

                return validLic;
            }
        }

        public abstract string FriendlyName { get; }

        public abstract string InformationString { get; }

        public bool Licensed
        {
            get
            {
                var validLic = false;
                try
                {
                    var lic = LicenseProvider.GetLicense(this.GetLicensePath(), this.GetSecretKey());
                    if (lic.Activated)
                    {
                        validLic = LicenseVerifier.IsValid(lic);
                        if (!validLic)
                        {
                            Report.Error(
                                pub => pub.DescriptionReason(ReportGenre.Licensing, $"License for '{this.FriendlyName}' was found but could not be validated."), new Exception("License found but could not be validated"));
                        }
                    }
                }
                catch (ApplicationException e)
                {
                    Report.Error(pub => pub.DescriptionReason(ReportGenre.Licensing, $"Licensing error in '{this.FriendlyName}'. {e.Message}"), e);
                }

                return validLic;
            }
        }

        public static void WaterMark<T>(T obj, IErrorListener<TraverserException> errorListener)
            where T : IContainer
        {
            const string watermarkText = " (Evaluation copy)";

            var desc = obj.SubObjects.OfType<Description>().SingleOrDefault();
            if (desc != null)
            {
                obj.AddSubObject(new Description(new XElement(desc.Basic.Value + watermarkText), desc.Detailed, desc.Lines));
                obj.SubObjects.Remove(desc);
            }
            else
            {
                obj.AddSubObject(new Description(new XElement("Basic", watermarkText), new XElement("Detailed", string.Empty), new List<FileLine>()));
            }
        }

        public string GetLicenseEmail()
        {
            return _lic.Email;
        }

        public string GetLicenseKey()
        {
            return _lic.Key;
        }

        public abstract string GetLicensePath();

        public abstract string GetSecretKey();

        public abstract void Initialize();

        public abstract int LoadOrder();
    }
}