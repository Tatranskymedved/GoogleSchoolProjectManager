using System.Reflection;
using System.Windows;

#if (DEBUG)
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Jan Urbanec")]
[assembly: AssemblyProduct("Google - School Project Manager")]
[assembly: AssemblyCopyright("Copyright © 2020")]
[assembly: AssemblyCulture("")]

//In order to begin building localizable applications, set
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]



// Version information for an assembly consists of the following four values:
// roughly translated from I reckon it is for SO, note that they most likely 
// dynamically generate this file
//      Major Version  - Year 6 being 2016
//      Minor Version  - The month
//      Day Number     - Day of month
//      Revision       - Build number
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below: [assembly: AssemblyVersion("year.month.day.*")]
[assembly: AssemblyVersion("2022.10.00.00")]
[assembly: AssemblyFileVersion("2022.10.30.0001")]
