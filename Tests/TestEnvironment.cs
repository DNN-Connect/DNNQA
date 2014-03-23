//
// Copyright (c) 2010
// by Will Morgenweck & Chris Paterra
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System.Collections.Generic;
using System.IO;

namespace DotNetNuke.DNNQA.Tests
{
    using System.Configuration;

    public static class DatabaseEnvironment
    {
        #region Properties
        public static string ConnectionString
        {
            get
            {
                return string.Format(TestDatabaseConnectionFormatString, TargetDatabaseFolderPath, TestDatabaseName);
            }
        }

        public static string TestDatabaseConnectionFormatString
        {
            get { return ConfigurationManager.AppSettings["TestDatabaseConnectionFormatString"]; }
        }

        public static string TestDatabaseName
        {
            get { return ConfigurationManager.AppSettings["TestDatabaseName"]; }
        }

        private static string _TargetDatabaseFolderPath;
        public static string TargetDatabaseFolderPath
        {
            get
            {
                if (_TargetDatabaseFolderPath == null)
                {
                    string configValue = ConfigurationManager.AppSettings["TargetDatabaseFolderPath"];
                    _TargetDatabaseFolderPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), configValue));
                }
                return _TargetDatabaseFolderPath;
            }
        }

        private static string _SourceDatabaseFolderPath;
        public static string SourceDatabaseFolderPath
        {
            get
            {
                if (_SourceDatabaseFolderPath == null)
                {
                    string configValue = ConfigurationManager.AppSettings["SourceDatabaseFolderPath"];
                    _SourceDatabaseFolderPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), configValue));
                }
                return _SourceDatabaseFolderPath;
            }
        }

        public static string TestDatabaseSetupScript
        {
            get { return ConfigurationManager.AppSettings["TestDatabaseSetupScript"]; }
        }

        private static List<string> _ModuleInstallScripts = null;
        public static List<string> ModuleInstallScripts
        {
            get
            {
                if (_ModuleInstallScripts == null)
                {
                    string configValue = ConfigurationManager.AppSettings["ModuleInstallScripts"];
                    _ModuleInstallScripts = new List<string>(configValue.Split(';'));

                }
                return _ModuleInstallScripts;
            }
        }

        public static string ModuleUnInstallScript
        {
            get { return ConfigurationManager.AppSettings["ModuleUnInstallScripts"]; }
        }

        public static string ObjectQualifier
        {
            get
            {
                return ConfigurationManager.AppSettings["ObjectQualifier"];
            }
        }

        public static string DatabaseOwner
        {
            get
            {
                return ConfigurationManager.AppSettings["DatabaseOwner"];
            }
        }
        #endregion

    }

}
