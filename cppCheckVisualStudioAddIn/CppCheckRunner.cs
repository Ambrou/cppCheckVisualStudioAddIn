using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using EnvDTE;

namespace Ambre.cppCheckVisualStudioAddIn
{
    class CppCheckRunner
    {
        internal void run(List<string> files, CppCheckConfiguration cppCheckConfiguration, OutputWindowPane cppCheckPanel)
        {
            String cppCheckArguments = "";

            foreach (String file in files)
            {
                cppCheckArguments += "\"" + file + "\" ";
            }
            runCppCheck(cppCheckConfiguration.CppCheckPathAndExe, cppCheckArguments, cppCheckPanel);
        }

        private void runCppCheck(String cppCheckPathAndExe, String cppCheckArguments, OutputWindowPane cppCheckPanel)
        {
            cppCheckThread = new System.Threading.Thread(delegate()
            {
                runThreadFunc(cppCheckPathAndExe, cppCheckArguments, cppCheckPanel);
            });
            cppCheckThread.Name = "cppCheck";
            cppCheckThread.Start();
        }

        private void runThreadFunc(String cppCheckPathAndExe, String cppCheckArguments, OutputWindowPane cppCheckPanel)
        {
            cppCheckPanel.OutputString("Starting analyzer with arguments: " + cppCheckArguments + "\n");
        }

        private System.Threading.Thread cppCheckThread = null;
    }
}
