using System;
using System.Collections.Generic;
using System.Text;

namespace Ambre.cppCheckVisualStudioAddIn
{
    class CppCheckConfiguration
    {
        public String CppCheckPathAndExe
        {
            set { cppCheckPathAndExe = value; }
            get { return cppCheckPathAndExe; }
        }

        private String cppCheckPathAndExe = "C:\\Program Files\\Cppcheck";
    }
}
