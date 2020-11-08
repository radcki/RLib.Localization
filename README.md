# RLib.Localization
Helpers for simple strongly typed localization of .net applications. Why? Because keeping magic strings distributed over multiple .resx files is just asking for trouble.

### Usage
1. Create file with recources you want to localize declared as static string
```csharp
public class FormField
{
    public static string Password = "Password";
}
```
2. Add "Localized" attributes with translations
```csharp
public class FormField
{
    [Localized("pl-PL", "Hasło")]
    [Localized("de-DE", "Passwort")]
    public static string Password = "Password";
}
```
3. (Optional) Add "BaseResourceCulture" attribute to parent class to hint culture for actual string value
```csharp
[BaseResourceCulture("en-GB")]
public class FormField
{
    [Localized("pl-PL", "Hasło")]
    [Localized("de-DE", "Passwort")]
    public static string Password = "Password";
}
```
This would be similiar as: 

```csharp
public class FormField
{
    [Localized("pl-PL", "Hasło")]
    [Localized("de-DE", "Passwort")]
    [Localized("en-GB", "Password")]
    public static string Password = "Password";
}
```
BaseResourceCulture attribute can be declared higher in class hierachy, for example:
```csharp
[BaseResourceCulture(KnownCulture.English)]
public partial class PageString
{
    public class Common
    {
        [Localized(KnownCulture.Polish, "Hasło")]
        public static string Password = "Password";
    }
    public class Register
    {
        [Localized(KnownCulture.Polish, "Potwierdź hasło")]
        public static string ConfirmPassword = "Confirm password";
    }
}
```


4. Reference localized resource in project using Localization.For() method:

```csharp
var label = Localization.For(() => FormField.Password);
```
```razor
<span>@Localization.For(() => FormField.Password)</span>
```

### Resource parameters
You can declare named parameters in localized strings, for example:

```csharp
[BaseResourceCulture(KnownCulture.English)]
public class Message
{
    [Localized(KnownCulture.Polish, "Cześć {name}!")]
    [Localized(KnownCulture.German, "Hallo {name}!")]
    public static string Hello = "Hello {name}!";
}
```
Then declare them by passing object with matching fields:

```csharp
var label = Localization.For(() => Message.Hello, new {name = "foo"}); 
// label == "Hello foo!"
```

### Specifing culture
By default CultureInfo.CurrentCulture is used for localization. To adjust current culture for incoming request just add app.UseRequestLocalization() to Startup.cs.
You can also force specific culture by passing is as parameter

```csharp
var label = Localization.For(() => Message.Hello, , new CultureInfo(KnownCulture.Polish), new {name = "foo"}); 
// label == "Cześć foo!"
```

### Field validation in MVC
Solution will not work for default validation attributes, as this is not a valid code:
 
```csharp
[Required(ErrorMessage = Localization.For(() => ValidationMessage.FieldRequired))]
public string Password
```
But you can simply use other model validation solution instead without loosing ModelState error binding, like [FluentValidation](https://fluentvalidation.net/):

```csharp
public class Validator : AbstractValidator<LoginForm>
{
    public Validator()
    {
        RuleFor(x => x.Password).NotEmpty().WithMessage(Localization.For(() => ValidationMessage.FieldRequired));
    }
}
```
