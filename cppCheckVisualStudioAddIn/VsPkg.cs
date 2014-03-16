// VsPkg.cs : Implementation of cppCheckVisualStudioAddIn
//

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Collections.Generic;

using Microsoft.Win32;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

using EnvDTE;

namespace Ambre.cppCheckVisualStudioAddIn
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the registration utility (regpkg.exe) that this class needs
    // to be registered as package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // A Visual Studio component can be registered under different regitry roots; for instance
    // when you debug your package you want to register it in the experimental hive. This
    // attribute specifies the registry root to use if no one is provided to regpkg.exe with
    // the /root switch.
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\8.0")]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration(false, "#110", "#112", "1.0", IconResourceID = 400)]
    // In order be loaded inside Visual Studio in a machine that has not the VS SDK installed, 
    // package needs to have a valid load key (it can be requested at 
    // http://msdn.microsoft.com/vstudio/extend/). This attributes tells the shell that this 
    // package has a load key embedded in its resources.
    [ProvideLoadKey("Standard", "1.0", "cppCheckVisualStudioAddIn", "Ambre", 1)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource(1000, 1)]
    [Guid(GuidList.guidcppCheckVisualStudioAddInPkgString)]
    public sealed class cppCheckVisualStudioAddIn : Package
    {

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public cppCheckVisualStudioAddIn()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();
            _dte = (EnvDTE.DTE)GetService(typeof(SDTE));

            // Add our command handlers for menu (commands must exist in the .ctc file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the cppCheck config menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidcppCheckVisualStudioAddInCmdSet, PkgCmdIDList.cmdidcppCheckConfig);
                MenuCommand menuItem = new MenuCommand( new EventHandler(onCppCheckConfig), menuCommandID );
                mcs.AddCommand( menuItem );

                // Create the command for the cppCheck parse file menu item.
                menuCommandID = new CommandID(GuidList.guidcppCheckVisualStudioAddInCmdSet, PkgCmdIDList.cmdidcppCheckParseFile);
                menuItem = new MenuCommand(new EventHandler(onCppCheckParseFile), menuCommandID);
                mcs.AddCommand(menuItem);

                // Create the command for the cppCheck parse project menu item.
                menuCommandID = new CommandID(GuidList.guidcppCheckVisualStudioAddInCmdSet, PkgCmdIDList.cmdidcppCheckParseProject);
                menuItem = new MenuCommand(new EventHandler(onCppCheckParseProjects), menuCommandID);
                mcs.AddCommand(menuItem);

                // Create the command for the cppCheck parse solution menu item.
                menuCommandID = new CommandID(GuidList.guidcppCheckVisualStudioAddInCmdSet, PkgCmdIDList.cmdidcppCheckParseSolution);
                menuItem = new MenuCommand(new EventHandler(onCppCheckParseSolution), menuCommandID);
                mcs.AddCommand(menuItem);

                // Create the Command for toolbar button
                menuCommandID = new CommandID(GuidList.guidcppCheckVisualStudioAddInCmdSet, PkgCmdIDList.cmdidMyZoom);
                menuItem = new MenuCommand(new EventHandler(onCppCheckConfig), menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void onCppCheckConfig(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;
            uiShell.ShowMessageBox(
                       0,
                       ref clsid,
                       "cppCheckVisualStudioAddIn",
                       string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out result);
        }

        private void onCppCheckParseFile(object sender, EventArgs e)
        {

        }

        private void onCppCheckParseProjects(object sender, EventArgs e)
        {
            try
            {
                object[] activeProjects = (object[])_dte.ActiveSolutionProjects;
                hasSelectedProjects(activeProjects);
                List<String> files = getFilesFromProjects(activeProjects);
                // Run analysis on each file
                //runAnalysis(files, currentConfig, true, _projectAnalysisOutputPane);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        private void hasSelectedProjects(object[] activeProjects)
        {
            if (activeProjects.Length == 0)
            {
                throw new System.Exception("Pas de projet sélectionné dans l'explorateur de solution - rien à vérifier.");
            }
        }

        private List<string> getFilesFromProjects(object[] activeProjects)
        {
            List<string> fileList = new List<string>();
            foreach (Project project in activeProjects)
            {
                if(isCppProject(project) == false)
                {
                    //project.Name;
                    continue;
                }
                fileList.AddRange(getFilesFromProject(project));
            }
            return fileList;
        }

        private List<string> getFilesFromProject(Project project)
        {
            List<String> fileList = new List<String>();
            fileList.AddRange(getFilesFromProjectItems(project.ProjectItems));
            //foreach (ProjectItem projectItem in project.ProjectItems)
            //{
            //    fileList.AddRange(getFilesFromItemProject(projectItem));
            //    //projectItem.ProjectItems
            //    //fileList.Add(projectItem.Name);
            //}
            return fileList;
        }

        private IEnumerable<string> getFilesFromProjectItems(ProjectItems projectItems)
        {
            List<String> fileList = new List<String>();
            foreach (ProjectItem projectItem in projectItems)
            {
                ProjectItems subProjectItems = projectItem.ProjectItems;
                if (subProjectItems.Count == 0)
                {
                    addFile(fileList, projectItem.Name);
                }
                else
                {
                    fileList.AddRange(getFilesFromProjectItems(subProjectItems));
                }
                
            }
            return fileList;
        }

        private void addFile(List<string> fileList, string fileName)
        {
            if (fileName.EndsWith(".cpp") == true)
            {
                fileList.Add(fileName);
            }
        }


        private bool isCppProject(Project project)
        {
            Type projectObjectType = project.GetType();
            string typeName = Microsoft.VisualBasic.Information.TypeName(project.Object);
            return (typeName == "VCProject");
        }

        private void onCppCheckParseSolution(object sender, EventArgs e)
        {

        }

        private DTE _dte = null;

    }
}