﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Portable.Xaml.Markup;
using QA.Validation.Xaml;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("72b2c29f-6d11-4881-ad30-026f22267811")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml", "QA.Validation.Xaml.Markup")]
[assembly: XmlnsDefinition("http://artq.com/validation", "QA.Validation.Xaml")]
[assembly: XmlnsDefinition("http://artq.com/validation", "QA.Validation.Xaml.ListTypes")]
[assembly: ValidationPlugin]

// Для тестов
[assembly: InternalsVisibleTo("QA.Validation.Xaml.Tests")]
[assembly: InternalsVisibleTo("QA.Validation.Xaml.Console")]
