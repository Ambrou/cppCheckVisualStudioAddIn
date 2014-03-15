// Guids.cs
// MUST match guids.h
using System;

namespace Ambre.cppCheckVisualStudioAddIn
{
    static class GuidList
    {
        public const string guidcppCheckVisualStudioAddInPkgString = "2d61439f-3473-46f4-b514-fcfa058992be";
        public const string guidcppCheckVisualStudioAddInCmdSetString = "835d0309-b686-4942-98d9-0e57fd96fd1a";

        public static readonly Guid guidcppCheckVisualStudioAddInPkg = new Guid(guidcppCheckVisualStudioAddInPkgString);
        public static readonly Guid guidcppCheckVisualStudioAddInCmdSet = new Guid(guidcppCheckVisualStudioAddInCmdSetString);
    };
}