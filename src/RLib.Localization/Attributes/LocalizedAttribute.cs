using System;
using System.Globalization;

namespace RLib.Localization.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class LocalizedAttribute : Attribute
    {
        public CultureInfo Culture { get; }
        public string Text { get; }

        public LocalizedAttribute(string cultureCode, string text)
        {
            this.Culture = new CultureInfo(cultureCode);
            this.Text = text;
        }
    }
}