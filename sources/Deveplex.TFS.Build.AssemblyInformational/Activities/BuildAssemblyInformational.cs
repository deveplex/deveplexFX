namespace Deveplex.TeamFoundation.Build.Activities
{
    using System;
    using System.Activities;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using Microsoft.TeamFoundation.Build.Client;

    [BuildActivity(HostEnvironmentOption.Agent)]
    public sealed class BuildAssemblyInformational : BuildCodeActivity
    {
        private BuildAction _Action;

        private bool _IsBuildAssemblyCompany;
        private bool _IsBuildAssemblyConfiguration;
        private bool _IsBuildAssemblyCopyright;
        private bool _IsBuildAssemblyCulture;
        private bool _IsBuildAssemblyDescription;
        private bool _IsBuildAssemblyFileVersion;
        private bool _IsBuildAssemblyInformationalVersion;
        private bool _IsBuildAssemblyProduct;
        private bool _IsBuildAssemblyTitle;
        private bool _IsBuildAssemblyTrademark;
        private bool _IsBuildAssemblyVersion;

        private Regex _RegexAssemblyCompany;
        private Regex _RegexAssemblyConfiguration;
        private Regex _RegexAssemblyCopyright;
        private Regex _RegexAssemblyCulture;
        private Regex _RegexAssemblyDescription;
        private Regex _RegexAssemblyProduct;
        private Regex _RegexAssemblyTitle;
        private Regex _RegexAssemblyTrademark;
        private Regex _RegexAssemblyVersion;
        private Regex _RegexAssemblyFileVersion;
        private Regex _RegexAssemblyInformationalVersion;
        private Regex _RegexAssemblyProductVersion;

        private Encoding _TextEncoding = Encoding.UTF8;

        private void ExecuteBuildCppAssemblyInformational()
        {
            //string assemblyTitleFormat = "<assembly: AssemblyTitle(\"{0}\")>";
            string assemblyDescriptionFormat = "VALUE \"FileDescription\", \"{0}\"\r\n";
            //string assemblyConfigurationFormat = "<assembly: AssemblyConfiguration(\"{0}\")>";
            string assemblyCompanyFormat = "VALUE \"CompanyName\", \"{0}\"\r\n";
            string assemblyProductFormat = "VALUE \"ProductName\", \"{0}\"\r\n";
            string assemblyCopyrightFormat = "VALUE \"LegalCopyright\", \"{0}\"\r\n";
            //string assemblyTrademarkFormat = "<assembly: AssemblyTrademark(\"{0}\")>";
            //string assemblyCultureFormat = "<assembly: AssemblyCulture(\"{0}\")>";
            string assemblyVersionFormat = "FILEVERSION {0}\r\n";
            string assemblyProductVersionFormat = "PRODUCTVERSION {0}\r\n";
            string assemblyFileVersionFormat = "VALUE \"FileVersion\", \"{0}\"\r\n";
            string assemblyInformationalVersionFormat = "VALUE \"ProductVersion\", \"{0}\"\r\n";

            this._RegexAssemblyVersion = new Regex(@"FILEVERSION.*[\.,].*[\.,].*[\.,].*[\r\n]", RegexOptions.Compiled);
            this._RegexAssemblyProductVersion = new Regex(@"PRODUCTVERSION.*[\.,].*[\.,].*[\.,].*[\r\n]", RegexOptions.Compiled);
            this._RegexAssemblyFileVersion = new Regex("VALUE \"FileVersion\".*\".*\".*[\\r\\n]", RegexOptions.Compiled);
            this._RegexAssemblyInformationalVersion = new Regex("VALUE \"ProductVersion\".*\".*\".*[\\r\\n]", RegexOptions.Compiled);
            this._RegexAssemblyDescription = new Regex("VALUE \"FileDescription\".*\".*\".*[\\r\\n]", RegexOptions.Compiled);
            this._RegexAssemblyCompany = new Regex("VALUE \"CompanyName\".*\".*\".*[\\r\\n]", RegexOptions.Compiled);
            this._RegexAssemblyProduct = new Regex("VALUE \"ProductName\".*\".*\".*[\\r\\n]", RegexOptions.Compiled);
            this._RegexAssemblyCopyright = new Regex("VALUE \"LegalCopyright\".*\".*\".*[\\r\\n]", RegexOptions.Compiled);

            if (base.ActivityContext.GetValue<IEnumerable<string>>(this.Files) == null)
            {
                base.LogBuildError("No Files specified. Pass an Item Collection of files to the Files property.");
            }
            else
            {
                foreach (string filename in base.ActivityContext.GetValue<IEnumerable<string>>(this.Files))
                {
                    string assemblyInfo = "";
                    FileInfo fileinfo = new FileInfo(filename);
                    FileAttributes attributes = File.GetAttributes(fileinfo.FullName);
                    bool flag = false;
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        base.LogBuildMessage("Making file writable", BuildMessageImportance.Low);
                        File.SetAttributes(fileinfo.FullName, attributes ^ FileAttributes.ReadOnly);
                        flag = true;
                    }
                    using (StreamReader reader = new StreamReader(fileinfo.FullName, true))
                    {
                        assemblyInfo = reader.ReadToEnd();
                    }
                    if (this._IsBuildAssemblyVersion)
                    {
                        assemblyInfo = this._RegexAssemblyVersion.Replace(assemblyInfo, string.Format(assemblyVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyVersion).Replace('.', ',')));
                        assemblyInfo = this._RegexAssemblyProductVersion.Replace(assemblyInfo, string.Format(assemblyProductVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyVersion).Replace('.', ',')));
                    }
                    if (this._IsBuildAssemblyFileVersion)
                    {
                        assemblyInfo = this._RegexAssemblyFileVersion.Replace(assemblyInfo, string.Format(assemblyFileVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyFileVersion)));
                    }
                    if (this._IsBuildAssemblyInformationalVersion)
                    {
                        assemblyInfo = this._RegexAssemblyInformationalVersion.Replace(assemblyInfo, string.Format(assemblyInformationalVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyInformationVersion)));
                    }
                    if (this._IsBuildAssemblyDescription)
                    {
                        assemblyInfo = this._RegexAssemblyDescription.Replace(assemblyInfo, string.Format(assemblyDescriptionFormat, base.ActivityContext.GetValue<string>(this.AssemblyDescription)));
                    }
                    if (this._IsBuildAssemblyCompany)
                    {
                        assemblyInfo = this._RegexAssemblyCompany.Replace(assemblyInfo, string.Format(assemblyCompanyFormat, base.ActivityContext.GetValue<string>(this.AssemblyCompany)));
                    }
                    if (this._IsBuildAssemblyProduct)
                    {
                        assemblyInfo = this._RegexAssemblyProduct.Replace(assemblyInfo, string.Format(assemblyProductFormat, base.ActivityContext.GetValue<string>(this.AssemblyProduct)));
                    }
                    if (this._IsBuildAssemblyCopyright)
                    {
                        assemblyInfo = this._RegexAssemblyCopyright.Replace(assemblyInfo, string.Format(assemblyCopyrightFormat, base.ActivityContext.GetValue<string>(this.AssemblyCopyright)));
                    }
                    using (StreamWriter writer = new StreamWriter(fileinfo.FullName, false, this._TextEncoding))
                    {
                        writer.Write(assemblyInfo);
                    }
                    if (flag)
                    {
                        base.LogBuildMessage("Making file readonly", BuildMessageImportance.Low);
                        File.SetAttributes(fileinfo.FullName, FileAttributes.ReadOnly);
                    }
                }
            }
        }

        private void ExecuteBuildCsAssemblyInformational()
        {
            string assemblyTitleFormat = "[assembly: AssemblyTitle(\"{0}\")]";
            string assemblyDescriptionFormat = "[assembly: AssemblyDescription(\"{0}\")]";
            string assemblyConfigurationFormat = "[assembly: AssemblyConfiguration(\"{0}\")]";
            string assemblyCompanyFormat = "[assembly: AssemblyCompany(\"{0}\")]";
            string assemblyProductFormat = "[assembly: AssemblyProduct(\"{0}\")]";
            string assemblyCopyrightFormat = "[assembly: AssemblyCopyright(\"{0}\")]";
            string assemblyTrademarkFormat = "[assembly: AssemblyTrademark(\"{0}\")]";
            string assemblyCultureFormat = "[assembly: AssemblyCulture(\"{0}\")]";
            string assemblyVersionFormat = "[assembly: AssemblyVersion(\"{0}\")]";
            string assemblyFileVersionFormat = "[assembly: AssemblyFileVersion(\"{0}\")]";
            string assemblyInformationalVersionFormat = "[assembly: AssemblyInformationalVersion(\"{0}\")]";

            this._RegexAssemblyVersion = new Regex(@"\[[\s]*assembly:[\s]*AssemblyVersion\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyFileVersion = new Regex(@"\[[\s]*assembly:[\s]*AssemblyFileVersion\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyInformationalVersion = new Regex(@"\[[\s]*assembly:[\s]*AssemblyInformationalVersion\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyTitle = new Regex(@"\[[\s]*assembly:[\s]*AssemblyTitle\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyDescription = new Regex(@"\[[\s]*assembly:[\s]*AssemblyDescription\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyConfiguration = new Regex(@"\[[\s]*assembly:[\s]*AssemblyConfiguration\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyCompany = new Regex(@"\[[\s]*assembly:[\s]*AssemblyCompany\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyProduct = new Regex(@"\[[\s]*assembly:[\s]*AssemblyProduct\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyCopyright = new Regex(@"\[[\s]*assembly:[\s]*AssemblyCopyright\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyTrademark = new Regex(@"\[[\s]*assembly:[\s]*AssemblyTrademark\(.*\)[\s]*\]", RegexOptions.Compiled);
            this._RegexAssemblyCulture = new Regex(@"\[[\s]*assembly:[\s]*AssemblyCulture\(.*\)[\s]*\]", RegexOptions.Compiled);

            if (base.ActivityContext.GetValue<IEnumerable<string>>(this.Files) == null)
            {
                base.LogBuildError("No Files specified. Pass an Item Collection of files to the Files property.");
            }
            else
            {
                foreach (string filename in base.ActivityContext.GetValue<IEnumerable<string>>(this.Files))
                {
                    string assemblyinfo = "";
                    string extension;
                    FileInfo fileinfo = new FileInfo(filename);
                    FileAttributes attributes = File.GetAttributes(fileinfo.FullName);
                    extension = fileinfo.Extension;
                    if (extension != ".cs")
                    {
                        base.LogBuildMessage(filename + " is not .cs file", BuildMessageImportance.Low);
                        break;
                    }
                    bool flag = false;
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        base.LogBuildMessage("Making file writable", BuildMessageImportance.Low);
                        File.SetAttributes(fileinfo.FullName, attributes ^ FileAttributes.ReadOnly);
                        flag = true;
                    }
                    using (StreamReader reader = new StreamReader(fileinfo.FullName, this._TextEncoding, true))
                    {
                        assemblyinfo = reader.ReadToEnd();
                    }
                    if (this._IsBuildAssemblyVersion)
                    {
                        Match match = this._RegexAssemblyVersion.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyVersion.Replace(assemblyinfo, string.Format(assemblyVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyVersion)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyVersionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyVersion)));
                        }
                    }
                    if (this._IsBuildAssemblyFileVersion)
                    {
                        Match match = this._RegexAssemblyFileVersion.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyFileVersion.Replace(assemblyinfo, string.Format(assemblyFileVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyFileVersion)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyFileVersionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyFileVersion)));
                        }
                    }
                    if (this._IsBuildAssemblyInformationalVersion)
                    {
                        Match match = this._RegexAssemblyInformationalVersion.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyInformationalVersion.Replace(assemblyinfo, string.Format(assemblyInformationalVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyInformationVersion)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyInformationalVersionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyInformationVersion)));
                        }
                    }
                    if (this._IsBuildAssemblyTitle)
                    {
                        Match match = this._RegexAssemblyTitle.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyTitle.Replace(assemblyinfo, string.Format(assemblyTitleFormat, base.ActivityContext.GetValue<string>(this.AssemblyTitle)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyTitleFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyTitle)));
                        }
                    }
                    if (this._IsBuildAssemblyDescription)
                    {
                        Match match = this._RegexAssemblyDescription.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyDescription.Replace(assemblyinfo, string.Format(assemblyDescriptionFormat, base.ActivityContext.GetValue<string>(this.AssemblyDescription)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyDescriptionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyDescription)));
                        }
                    }
                    if (this._IsBuildAssemblyConfiguration)
                    {
                        Match match = this._RegexAssemblyConfiguration.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyConfiguration.Replace(assemblyinfo, string.Format(assemblyConfigurationFormat, base.ActivityContext.GetValue<string>(this.AssemblyConfiguration)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyConfigurationFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyConfiguration)));
                        }
                    }
                    if (this._IsBuildAssemblyCompany)
                    {
                        Match match = this._RegexAssemblyCompany.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyCompany.Replace(assemblyinfo, string.Format(assemblyCompanyFormat, base.ActivityContext.GetValue<string>(this.AssemblyCompany)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyCompanyFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyCompany)));
                        }
                    }
                    if (this._IsBuildAssemblyProduct)
                    {
                        Match match = this._RegexAssemblyProduct.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyProduct.Replace(assemblyinfo, string.Format(assemblyProductFormat, base.ActivityContext.GetValue<string>(this.AssemblyProduct)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyProductFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyProduct)));
                        }
                    }
                    if (this._IsBuildAssemblyCopyright)
                    {
                        Match match = this._RegexAssemblyCopyright.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyCopyright.Replace(assemblyinfo, string.Format(assemblyCopyrightFormat, base.ActivityContext.GetValue<string>(this.AssemblyCopyright)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyCopyrightFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyCopyright)));
                        }
                    }
                    if (this._IsBuildAssemblyTrademark)
                    {
                        Match match = this._RegexAssemblyTrademark.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyTrademark.Replace(assemblyinfo, string.Format(assemblyTrademarkFormat, base.ActivityContext.GetValue<string>(this.AssemblyTrademark)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyTrademarkFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyTrademark)));
                        }
                    }
                    if (this._IsBuildAssemblyCulture)
                    {
                        Match match = this._RegexAssemblyCulture.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyCulture.Replace(assemblyinfo, string.Format(assemblyCultureFormat, base.ActivityContext.GetValue<string>(this.AssemblyCulture)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyCultureFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyCulture)));
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(fileinfo.FullName, false, this._TextEncoding))
                    {
                        writer.Write(assemblyinfo);
                    }
                    if (flag)
                    {
                        base.LogBuildMessage("Making file readonly", BuildMessageImportance.Low);
                        File.SetAttributes(fileinfo.FullName, FileAttributes.ReadOnly);
                    }
                }
            }
        }

        private void ExecuteBuildVbAssemblyInformational()
        {
            string assemblyTitleFormat = "<assembly: AssemblyTitle(\"{0}\")>";
            string assemblyDescriptionFormat = "<assembly: AssemblyDescription(\"{0}\")>";
            string assemblyConfigurationFormat = "<assembly: AssemblyConfiguration(\"{0}\")>";
            string assemblyCompanyFormat = "<assembly: AssemblyCompany(\"{0}\")>";
            string assemblyProductFormat = "<assembly: AssemblyProduct(\"{0}\")>";
            string assemblyCopyrightFormat = "<assembly: AssemblyCopyright(\"{0}\")>";
            string assemblyTrademarkFormat = "<assembly: AssemblyTrademark(\"{0}\")>";
            string assemblyCultureFormat = "<assembly: AssemblyCulture(\"{0}\")>";
            string assemblyVersionFormat = "<assembly: AssemblyVersion(\"{0}\")>";
            string assemblyFileVersionFormat = "<assembly: AssemblyFileVersion(\"{0}\")>";
            string assemblyInformationalVersionFormat = "<assembly: AssemblyInformationalVersion(\"{0}\")>";

            this._RegexAssemblyVersion = new Regex(@"\<[\s]*assembly:[\s]*AssemblyVersion\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyFileVersion = new Regex(@"\<[\s]*assembly:[\s]*AssemblyFileVersion\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyInformationalVersion = new Regex(@"\<[\s]*assembly:[\s]*AssemblyInformationalVersion\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyTitle = new Regex(@"\<[\s]*assembly:[\s]*AssemblyTitle\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyDescription = new Regex(@"\<[\s]*assembly:[\s]*AssemblyDescription\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyConfiguration = new Regex(@"\<[\s]*assembly:[\s]*AssemblyConfiguration\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyCompany = new Regex(@"\<[\s]*assembly:[\s]*AssemblyCompany\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyProduct = new Regex(@"\<[\s]*assembly:[\s]*AssemblyProduct\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyCopyright = new Regex(@"\<[\s]*assembly:[\s]*AssemblyCopyright\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyTrademark = new Regex(@"\<[\s]*assembly:[\s]*AssemblyTrademark\(.*\)[\s]*\>", RegexOptions.Compiled);
            this._RegexAssemblyCulture = new Regex(@"\<[\s]*assembly:[\s]*AssemblyCulture\(.*\)[\s]*\>", RegexOptions.Compiled);

            if (base.ActivityContext.GetValue<IEnumerable<string>>(this.Files) == null)
            {
                base.LogBuildError("No Files specified. Pass an Item Collection of files to the Files property.");
            }
            else
            {
                foreach (string filename in base.ActivityContext.GetValue<IEnumerable<string>>(this.Files))
                {
                    string assemblyinfo = "";
                    string extension;
                    FileInfo fileinfo = new FileInfo(filename);
                    FileAttributes attributes = File.GetAttributes(fileinfo.FullName);
                    extension = fileinfo.Extension;
                    if (extension != ".vb")
                    {
                        base.LogBuildMessage(filename + " is not .vb file", BuildMessageImportance.Low);
                        break;
                    }
                    bool flag = false;
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        base.LogBuildMessage("Making file writable", BuildMessageImportance.Low);
                        File.SetAttributes(fileinfo.FullName, attributes ^ FileAttributes.ReadOnly);
                        flag = true;
                    }
                    using (StreamReader reader = new StreamReader(fileinfo.FullName, this._TextEncoding, true))
                    {
                        assemblyinfo = reader.ReadToEnd();
                    }
                    if (this._IsBuildAssemblyVersion)
                    {
                        Match match = this._RegexAssemblyVersion.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyVersion.Replace(assemblyinfo, string.Format(assemblyVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyVersion)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyVersionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyVersion)));
                        }
                    }
                    if (this._IsBuildAssemblyFileVersion)
                    {
                        Match match = this._RegexAssemblyFileVersion.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyFileVersion.Replace(assemblyinfo, string.Format(assemblyFileVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyFileVersion)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyFileVersionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyFileVersion)));
                        }
                    }
                    if (this._IsBuildAssemblyInformationalVersion)
                    {
                        Match match = this._RegexAssemblyInformationalVersion.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyInformationalVersion.Replace(assemblyinfo, string.Format(assemblyInformationalVersionFormat, base.ActivityContext.GetValue<string>(this.AssemblyInformationVersion)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyInformationalVersionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyInformationVersion)));
                        }
                    }
                    if (this._IsBuildAssemblyTitle)
                    {
                        Match match = this._RegexAssemblyTitle.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyTitle.Replace(assemblyinfo, string.Format(assemblyTitleFormat, base.ActivityContext.GetValue<string>(this.AssemblyTitle)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyTitleFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyTitle)));
                        }
                    }
                    if (this._IsBuildAssemblyDescription)
                    {
                        Match match = this._RegexAssemblyDescription.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyDescription.Replace(assemblyinfo, string.Format(assemblyDescriptionFormat, base.ActivityContext.GetValue<string>(this.AssemblyDescription)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyDescriptionFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyDescription)));
                        }
                    }
                    if (this._IsBuildAssemblyConfiguration)
                    {
                        Match match = this._RegexAssemblyConfiguration.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyConfiguration.Replace(assemblyinfo, string.Format(assemblyConfigurationFormat, base.ActivityContext.GetValue<string>(this.AssemblyConfiguration)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyConfigurationFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyConfiguration)));
                        }
                    }
                    if (this._IsBuildAssemblyCompany)
                    {
                        Match match = this._RegexAssemblyCompany.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyCompany.Replace(assemblyinfo, string.Format(assemblyCompanyFormat, base.ActivityContext.GetValue<string>(this.AssemblyCompany)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyCompanyFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyCompany)));
                        }
                    }
                    if (this._IsBuildAssemblyProduct)
                    {
                        Match match = this._RegexAssemblyProduct.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyProduct.Replace(assemblyinfo, string.Format(assemblyProductFormat, base.ActivityContext.GetValue<string>(this.AssemblyProduct)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyProductFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyProduct)));
                        }
                    }
                    if (this._IsBuildAssemblyCopyright)
                    {
                        Match match = this._RegexAssemblyCopyright.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyCopyright.Replace(assemblyinfo, string.Format(assemblyCopyrightFormat, base.ActivityContext.GetValue<string>(this.AssemblyCopyright)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyCopyrightFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyCopyright)));
                        }
                    }
                    if (this._IsBuildAssemblyTrademark)
                    {
                        Match match = this._RegexAssemblyTrademark.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyTrademark.Replace(assemblyinfo, string.Format(assemblyTrademarkFormat, base.ActivityContext.GetValue<string>(this.AssemblyTrademark)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyTrademarkFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyTrademark)));
                        }
                    }
                    if (this._IsBuildAssemblyCulture)
                    {
                        Match match = this._RegexAssemblyCulture.Match(assemblyinfo);
                        if (match.Success)
                        {
                            assemblyinfo = this._RegexAssemblyCulture.Replace(assemblyinfo, string.Format(assemblyCultureFormat, base.ActivityContext.GetValue<string>(this.AssemblyCulture)));
                        }
                        else
                        {
                            assemblyinfo = assemblyinfo.Append(string.Format(assemblyCultureFormat + "\n", base.ActivityContext.GetValue<string>(this.AssemblyCulture)));
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(fileinfo.FullName, false, this._TextEncoding))
                    {
                        writer.Write(assemblyinfo);
                    }
                    if (flag)
                    {
                        base.LogBuildMessage("Making file readonly", BuildMessageImportance.Low);
                        File.SetAttributes(fileinfo.FullName, FileAttributes.ReadOnly);
                    }
                }
            }
        }

        protected override void InternalExecute()
        {
            switch (this.Action)
            {
                case BuildAction.BuildCppAssemblyInformational:
                    this.ExecuteBuildCppAssemblyInformational();
                    break;
                case BuildAction.BuildCsAssemblyInformational:
                    this.ExecuteBuildCsAssemblyInformational();
                    break;
                case BuildAction.BuildVbAssemblyInformational:
                    this.ExecuteBuildVbAssemblyInformational();
                    break;
                default:
                    throw new ArgumentException("Action not supported");
                    break;
            }
        }

        [Category("操作"), DisplayName("生成操作"), Description("生成操作")]
        public BuildAction Action
        {
            get
            {
                return this._Action;
            }
            set
            {
                this._Action = value;
            }
        }

        [Browsable(false), Description("Set to true to make all warnings errors")]
        public InArgument<IEnumerable<string>> Files { get; set; }

        [Category("程序集信息"), DisplayName("公司名称"), Description("公司名称，可以是任何合法的字符串，可以有空格。")]
        public InArgument<string> AssemblyCompany { get; set; }

        [Category("程序集信息"), DisplayName("程序集配置信息"), Description("程序集的配置信息，可以是任何合法的字符串，可以有空格。")]
        public InArgument<string> AssemblyConfiguration { get; set; }

        [Category("程序集信息"), DisplayName("程序集版权信息"), Description("程序集的版权信息，可以是任何合法的字符串，可以有空格。")]
        public InArgument<string> AssemblyCopyright { get; set; }

        [Category("程序集信息"), DisplayName("程序集区域信息"), Description("程序集的区域信息，枚举的字段表明程序集支持的区域性。程序集也可以指定区域独立性，表明它包含用于默认区域性的资源。运行库将任何区域性属性未设为空的程序集按附属程序集处理。此类程序集受附属程序集绑定规则约束。")]
        public InArgument<string> AssemblyCulture { get; set; }

        [Category("程序集信息"), DisplayName("程序集简单描述"), Description("程序集的简单描述，可以是任何合法的字符串，可以有空格。")]
        public InArgument<string> AssemblyDescription { get; set; }

        [Category("程序集信息"), DisplayName("文件版本号"), Description("文件的版本号")]
        public InOutArgument<string> AssemblyFileVersion { get; set; }

        [Category("程序集信息"), DisplayName("产品版本号"), Description("产品的版本号")]
        public InOutArgument<string> AssemblyInformationVersion { get; set; }

        [Category("程序集信息"), DisplayName("产品名称"), Description("产品名称，可以是任何合法的字符串，可以有空格。")]
        public InArgument<string> AssemblyProduct { get; set; }

        [Category("程序集信息"), DisplayName("程序集名称"), Description("描述程序集的名称，可以是任何合法的字符串，可以有空格。")]
        public InArgument<string> AssemblyTitle { get; set; }

        [Category("程序集信息"), DisplayName("程序集商标信息"), Description("程序集的商标信息，可以是任何合法的字符串，可以有空格。")]
        public InArgument<string> AssemblyTrademark { get; set; }

        [Category("程序集信息"), DisplayName("程序集版本号"), Description("程序集的版本号")]
        public InArgument<string> AssemblyVersion { get; set; }

        [Category("操作"), DisplayName("是否生成公司名称"), Description("是否生成公司名称")]
        public bool IsBuildAssemblyCompany
        {
            get
            {
                return this._IsBuildAssemblyCompany;
            }
            set
            {
                this._IsBuildAssemblyCompany = value;
            }
        }

        [Category("操作"), DisplayName("是否生成程序集配置信息"), Description("是否生成程序集配置信息")]
        public bool IsBuildAssemblyConfiguration
        {
            get
            {
                return this._IsBuildAssemblyConfiguration;
            }
            set
            {
                this._IsBuildAssemblyConfiguration = value;
            }
        }

        [Category("操作"), DisplayName("是否生成程序集版权信息"), Description("是否生成程序集版权信息")]
        public bool IsBuildAssemblyCopyright
        {
            get
            {
                return this._IsBuildAssemblyCopyright;
            }
            set
            {
                this._IsBuildAssemblyCopyright = value;
            }
        }

        [Category("操作"), DisplayName("是否生成程序集区域信息"), Description("是否生成程序集区域信息")]
        public bool IsBuildAssemblyCulture
        {
            get
            {
                return this._IsBuildAssemblyCulture;
            }
            set
            {
                this._IsBuildAssemblyCulture = value;
            }
        }

        [Category("操作"), DisplayName("是否生成程序集简单描述"), Description("是否生成程序集简单描述")]
        public bool IsBuildAssemblyDescription
        {
            get
            {
                return this._IsBuildAssemblyDescription;
            }
            set
            {
                this._IsBuildAssemblyDescription = value;
            }
        }

        [Category("操作"), DisplayName("是否生成文件版本号"), Description("是否生成文件版本号")]
        public bool IsBuildAssemblyFileVersion
        {
            get
            {
                return this._IsBuildAssemblyFileVersion;
            }
            set
            {
                this._IsBuildAssemblyFileVersion = value;
            }
        }

        [Category("操作"), DisplayName("是否生成产品版本号"), Description("是否生成产品版本号")]
        public bool IsBuildAssemblyInformationalVersion
        {
            get
            {
                return this._IsBuildAssemblyInformationalVersion;
            }
            set
            {
                this._IsBuildAssemblyInformationalVersion = value;
            }
        }

        [Category("操作"), DisplayName("是否生成产品名称"), Description("是否生成产品名称")]
        public bool IsBuildAssemblyProduct
        {
            get
            {
                return this._IsBuildAssemblyProduct;
            }
            set
            {
                this._IsBuildAssemblyProduct = value;
            }
        }

        [Category("操作"), DisplayName("是否生成程序集名称"), Description("是否生成程序集名称")]
        public bool IsBuildAssemblyTitle
        {
            get
            {
                return this._IsBuildAssemblyTitle;
            }
            set
            {
                this._IsBuildAssemblyTitle = value;
            }
        }

        [Category("操作"), DisplayName("是否生成程序集商标声明"), Description("是否生成程序集商标声明")]
        public bool IsBuildAssemblyTrademark
        {
            get
            {
                return this._IsBuildAssemblyTrademark;
            }
            set
            {
                this._IsBuildAssemblyTrademark = value;
            }
        }

        [Category("操作"), DisplayName("是否生成程序集版本号"), Description("是否生成程序集版本号")]
        public bool IsBuildAssemblyVersion
        {
            get
            {
                return this._IsBuildAssemblyVersion;
            }
            set
            {
                this._IsBuildAssemblyVersion = value;
            }
        }

        [Category("杂项"), DisplayName("文件编码格式"), Description("文件编码格式，默认为Unicode")]
        public TextEncoding TextEncoding
        {
            get
            {
                if (this._TextEncoding == Encoding.ASCII)
                    return TextEncoding.ASCII;
                if (this._TextEncoding == Encoding.BigEndianUnicode)
                    return TextEncoding.BigEndianUnicode;
                if (this._TextEncoding == Encoding.UTF7)
                    return TextEncoding.UTF7;
                if (this._TextEncoding == Encoding.UTF8)
                    return TextEncoding.UTF8;
                if (this._TextEncoding == Encoding.UTF32)
                    return TextEncoding.UTF32;
                else
                    return TextEncoding.Unicode;
            }
            set
            {
                switch (value)
                {
                    case TextEncoding.ASCII:
                        this._TextEncoding = Encoding.ASCII;
                        break;
                    case TextEncoding.BigEndianUnicode:
                        this._TextEncoding = Encoding.BigEndianUnicode;
                        break;
                    case TextEncoding.UTF7:
                        this._TextEncoding = Encoding.UTF7;
                        break;
                    case TextEncoding.UTF8:
                        this._TextEncoding = Encoding.UTF8;
                        break;
                    case TextEncoding.UTF32:
                        this._TextEncoding = Encoding.UTF32;
                        break;
                    default:
                        this._TextEncoding = Encoding.Unicode;
                        break;
                }
            }
        }
    }
}

