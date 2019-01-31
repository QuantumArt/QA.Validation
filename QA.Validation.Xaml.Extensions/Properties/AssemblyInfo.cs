using System.Reflection;
using System.Runtime.InteropServices;
#if NET_STANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif
using QA.Validation.Xaml;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("41a1025e-4991-4259-b439-94f9e8dbeb3f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: XmlnsDefinition("http://artq.com/validation", "QA.Validation.Xaml.Extensions")]
[assembly: XmlnsDefinition("http://artq.com/validation", "QA.Validation.Xaml.Extensions.Conditions")]
[assembly: XmlnsDefinition("http://artq.com/validation", "QA.Validation.Xaml.Extensions.Rules")]
[assembly: XmlnsDefinition("http://artq.com/validation", "QA.Validation.Xaml.Extensions.ValueArguments")]

[assembly: ValidationPlugin]
