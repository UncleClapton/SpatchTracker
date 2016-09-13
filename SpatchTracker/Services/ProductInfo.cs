using System;
using System.Reflection;

namespace SpatchTracker.Services
{
    public class ProductInfo
    {
        private readonly Assembly assembly = Assembly.GetExecutingAssembly();
        private string _Title;
        private string _Description;
        private string _Company;
        private string _Product;
        private string _Copyright;
        private string _Trademark;
        private Version _Version;
        private string _VersionString;

        public string Title
        {
            get { return this._Title ?? (this._Title = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(this.assembly, typeof(AssemblyTitleAttribute))).Title); }
        }

        public string Description
        {
            get { return this._Description ?? (this._Description = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(this.assembly, typeof(AssemblyDescriptionAttribute))).Description); }
        }

        public string Company
        {
            get { return this._Company ?? (this._Company = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(this.assembly, typeof(AssemblyCompanyAttribute))).Company); }
        }

        public string Product
        {
            get { return this._Product ?? (this._Product = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(this.assembly, typeof(AssemblyProductAttribute))).Product); }
        }

        public string Copyright
        {
            get { return this._Copyright ?? (this._Copyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(this.assembly, typeof(AssemblyCopyrightAttribute))).Copyright); }
        }

        public string Trademark
        {
            get { return this._Trademark ?? (this._Trademark = ((AssemblyTrademarkAttribute)Attribute.GetCustomAttribute(this.assembly, typeof(AssemblyTrademarkAttribute))).Trademark); }
        }

        public Version Version
        {
            get { return this._Version ?? (this._Version = this.assembly.GetName().Version); }
        }

        public string VersionString
        {
            get
            {
                return this._VersionString ?? (this._VersionString = string.Format(
                    //v{Major.Minor}{.Build}?{rRevision}?{ Debug / Beta}?
                    "v{0}{1}{2}{3}",
                    this.Version.ToString(2),
                    this.Version.Build == 0 ? "" : "." + this.Version.Build,
                    this.Version.Revision == 0 ? "" : " r" + this.Version.Revision,
                    this.IsDebug ? " DEBUG" : this.IsBeta ? " BETA" : ""
                    ));
            }
        }

        public bool IsBeta
        {
#if BETA
			get { return true; }
#else
            get { return false; }
#endif
        }

        public bool IsDebug
        {
#if DEBUG
            get { return true; }
#else
			get { return false; }
#endif
        }
    }
}
