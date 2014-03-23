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

using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Text.RegularExpressions;

namespace DotNetNuke.DNNQA.Tests.Utilities
{
    public static class DatabaseManager
    {

        #region Public Methods

        public static List<int> GetMockModules()
        {
            var modules = new List<int>();
            var sproc = "{databaseOwner}{objectQualifier}Modules_GetModules".Replace("{objectQualifier}", DatabaseEnvironment.ObjectQualifier).Replace("{databaseOwner}", DatabaseEnvironment.DatabaseOwner);
            using (IDataReader reader = SqlHelper.ExecuteReader(DatabaseEnvironment.ConnectionString, CommandType.StoredProcedure, sproc))
            {
                while (reader.Read())
                    modules.Add((int)reader["ModuleID"]);
            }
            return modules;
        }

        public static List<int> GetMockUserIds()
        {
            var users = new List<int>();
            var sproc = "{databaseOwner}{objectQualifier}Users_GetUsers".Replace("{objectQualifier}", DatabaseEnvironment.ObjectQualifier).Replace("{databaseOwner}", DatabaseEnvironment.DatabaseOwner);
            using (IDataReader reader = SqlHelper.ExecuteReader(DatabaseEnvironment.ConnectionString, CommandType.StoredProcedure, sproc))
            {
                while (reader.Read())
                    users.Add((int)reader["UserID"]);
            }
            return users;
        }

        #endregion

        #region Private Methods

        public static void ReApplyScripts()
        {
            string script;
            script = File.ReadAllText(Path.Combine(DatabaseEnvironment.SourceDatabaseFolderPath, DatabaseEnvironment.TestDatabaseSetupScript));
            RunScript(script);
            foreach (var scriptPath in DatabaseEnvironment.ModuleInstallScripts)
            {
                script = File.ReadAllText(Path.Combine(DatabaseEnvironment.SourceDatabaseFolderPath, scriptPath));
                RunScript(script);
            }
        }

        private static void RunScript(string script)
        {
            script = script.Replace("{objectQualifier}", DatabaseEnvironment.ObjectQualifier).Replace("{databaseOwner}", DatabaseEnvironment.DatabaseOwner);

            var scripts = SqlDelimiterRegex.Split(script);
            foreach (var item in scripts)
            {
                using (var conn = new SqlConnection(DatabaseEnvironment.ConnectionString))
                {
                    var cmd = new SqlCommand(item, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        #endregion

        #region Private Fields

        // Following line adapted from the DotNetNuke.Data.SqlDataProvider SqlDelimiterRegex property
        private static readonly Regex SqlDelimiterRegex = new Regex(@"(?<=(?:[^\w]+|^))GO(?=(?: |\t)*?(?:\r?\n|$))", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        #endregion

    }

}
