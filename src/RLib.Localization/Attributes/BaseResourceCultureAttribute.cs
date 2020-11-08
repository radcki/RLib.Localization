using System;
using System.Globalization;

namespace RLib.Localization.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BaseResourceCultureAttribute : Attribute
    {
        public CultureInfo Culture { get; } 
        public BaseResourceCultureAttribute(string culture)
        {
            Culture = new CultureInfo(culture);
        }
    }
}