using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RLib.Localization.Attributes;

namespace RLib.Localization
{
    public static class Localization
    { 
        public static string For<T>(Expression<Func<T>> exp, object args = null)
        {
            return For(exp, CultureInfo.CurrentCulture, args);
        }

        public static string For<T>(Expression<Func<T>> exp, CultureInfo culture, object args = null)
        {
            var expr = (MemberExpression) exp.Body;

            var memberInfo = expr.Member;

            var parentMember = memberInfo.DeclaringType;
            var defaultCultureAttributes = new List<BaseResourceCultureAttribute>();
            while (parentMember != null)
            {
                defaultCultureAttributes = parentMember.GetCustomAttributes<BaseResourceCultureAttribute>(true).ToList();
                if (defaultCultureAttributes.Any())
                {
                    break;
                }

                parentMember = parentMember.DeclaringType;
            }

            var defaultResourceCulture = defaultCultureAttributes.FirstOrDefault()?.Culture;
            var valueAttributes = (LocalizedAttribute[]) memberInfo.GetCustomAttributes(typeof(LocalizedAttribute), true);

            string text;
            if (Equals(culture, defaultResourceCulture) || !valueAttributes.Any())
            {
                text = exp.Compile().Invoke().ToString();
            }
            else
            {
                text = valueAttributes.FirstOrDefault(x => Equals(x.Culture, culture))?.Text;
            }

            if (!string.IsNullOrEmpty(text) && args != null)
            {
                var properties = args.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var key = propertyInfo.Name;
                    var value = propertyInfo.GetValue(args)?.ToString();
                    if (!text.Contains("{" + key + "}"))
                    {
                        throw new ArgumentException($"Localized text did not contain formatting key '{key}' in translation for culture {culture.Name}.", key);
                    }
                    text = text.Replace("{" + key + "}", value);
                }
            }

            return text;
        }
    }
}