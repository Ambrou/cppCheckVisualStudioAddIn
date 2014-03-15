// guids.h: definitions of GUIDs/IIDs/CLSIDs used in this VsPackage

/*
Do not use #pragma once, as this file needs to be included twice.  Once to declare the externs
for the GUIDs, and again right after including initguid.h to actually define the GUIDs.
*/



// package guid
// { 2d61439f-3473-46f4-b514-fcfa058992be }
#define guidcppCheckVisualStudioAddInPkg { 0x2D61439F, 0x3473, 0x46F4, { 0xB5, 0x14, 0xFC, 0xFA, 0x5, 0x89, 0x92, 0xBE } }
#ifdef DEFINE_GUID
DEFINE_GUID(CLSID_cppCheckVisualStudioAddIn,
0x2D61439F, 0x3473, 0x46F4, 0xB5, 0x14, 0xFC, 0xFA, 0x5, 0x89, 0x92, 0xBE );
#endif

// Command set guid for our commands (used with IOleCommandTarget)
// { 835d0309-b686-4942-98d9-0e57fd96fd1a }
#define guidcppCheckVisualStudioAddInCmdSet { 0x835D0309, 0xB686, 0x4942, { 0x98, 0xD9, 0xE, 0x57, 0xFD, 0x96, 0xFD, 0x1A } }
#ifdef DEFINE_GUID
DEFINE_GUID(CLSID_cppCheckVisualStudioAddInCmdSet, 
0x835D0309, 0xB686, 0x4942, 0x98, 0xD9, 0xE, 0x57, 0xFD, 0x96, 0xFD, 0x1A );
#endif


