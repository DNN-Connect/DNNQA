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

using System;
using DotNetNuke.DNNQA;
using System.Data;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.DNNQA.Tests.Utilities;
using MbUnit.Framework;
using System.Collections.Generic;

namespace DotNetNuke.DNNQA.Tests {

	/// <summary>
	/// Summary description for SqlDataProviderFixture
	/// </summary>
	[TestFixture]
	public class SqlDataProviderFixture {

		#region Fields/Properties

		////private const int INT_ModuleID = 321;
		//private const string STR_Content = "Simple Sample Content";
		//private const string STR_UpdatedContent = "Updated Content";
		//private const string STR_COLUMN_NAME_Content = "Content";

		private List<int> _mockModules;
		private List<int> _mockUserIds;
		private SqlDataProvider _sqlDataProvider;

		#endregion

		#region Setup/TearDown

		private Boolean _scriptsApplied;

		[FixtureSetUp]
		public void TestSetup() {
			// SetUp Test Database
			if (!_scriptsApplied) {
				// NOTE: It appears that MbUnit runs the FixtureSetUp twice (with DevExpress Unit Test Runner),
				//       but we want to preserve the database changes so that they can be manually checked if
				//       needed.
				DatabaseManager.ReApplyScripts();
				_scriptsApplied = true;
			}
			_mockModules = DatabaseManager.GetMockModules();
			_mockUserIds = DatabaseManager.GetMockUserIds();

			// SetUp SqlDataProvider
			_sqlDataProvider = new SqlDataProvider
			                  	{
			                  		ConnectionString = DatabaseEnvironment.ConnectionString,
			                  		ObjectQualifier = DatabaseEnvironment.ObjectQualifier,
			                  		DatabaseOwner = DatabaseEnvironment.DatabaseOwner
			                  	};
		}

		#endregion

		[Test]
		public void MockModuleIDsShouldExist() {
			Assert.IsNotNull(_mockModules);
			Assert.GreaterThan<int>(_mockModules.Count, 0);
		}

		[Test]
		public void MockUserIDsShouldExist() {
			Assert.IsNotNull(_mockUserIds);
			Assert.GreaterThan<int>(_mockUserIds.Count, 0);
		}

		[Test]
		[Description("Adds and retrieves a TestDrivenModule record in the database")]
		public void AddTestDrivenModule() {
			//// Arrange
			//var actualContent = "";

			//// Act - Add
			//int itemId = _sqlDataProvider.AddTestDrivenDNNModule(_mockModules[0], STR_Content, _mockUserIds[0]);

			//// Assert - Add
			//Assert.IsTrue(itemId > 0);

			//// Act - Read
			//using (IDataReader dataReader = _sqlDataProvider.GetTestDrivenDNNModule(_mockModules[0], itemId)) {
			//     // Assert - Read
			//     Assert.IsTrue(dataReader.Read());
			//     // Act - Module Content
			//     actualContent = (string)dataReader[STR_COLUMN_NAME_Content];
			//     // Assert - Module Content
			//     Assert.AreEqual<string>(STR_Content, actualContent);
			//}
		}

		[Test]
		[Description("Adds, retrieves and then deletes a TestDrivenModule record in the database")]
		public void DeleteTestDrivenModule() {
			//var content = "";

			//int itemId = _sqlDataProvider.AddTestDrivenDNNModule(_mockModules[0], STR_Content, _mockUserIds[0]);
			//Assert.IsTrue(itemId > 0);

			//using (IDataReader dataReader = _sqlDataProvider.GetTestDrivenDNNModule(_mockModules[0], itemId)) {
			//     if (dataReader.Read())
			//          content = (string)dataReader[STR_COLUMN_NAME_Content];
			//}
			//Assert.AreEqual<string>(STR_Content, content);

			//_sqlDataProvider.DeleteTestDrivenDNNModule(_mockModules[0], itemId);

			//using (IDataReader dataReader = _sqlDataProvider.GetTestDrivenDNNModule(_mockModules[0], itemId)) {
			//     Assert.IsFalse(dataReader.Read());
			//}
		}

		[Test]
		[Description("Adds, updates and retrieves a TestDrivenModule record in the database")]
		public void UpdateTestDrivenDNNModule() {
			//var content = "";

			//int itemId = _sqlDataProvider.AddTestDrivenDNNModule(_mockModules[0], STR_Content, _mockUserIds[0]);
			//Assert.IsTrue(itemId > 0);

			//_sqlDataProvider.UpdateTestDrivenDNNModule(_mockModules[0], itemId, STR_UpdatedContent, _mockUserIds[0]);

			//using (IDataReader dataReader = _sqlDataProvider.GetTestDrivenDNNModule(_mockModules[0], itemId)) {
			//     if (dataReader.Read())
			//          content = (string)dataReader[STR_COLUMN_NAME_Content];
			//}
			//Assert.AreEqual<string>(STR_UpdatedContent, content);
		}

		[Test]
		[Description("Adds 2 and then retrieves 2 TestDrivenModule records in the database")]
		public void GetTestDrivenDNNModules() {
			//var records = 0;

			//int itemId1 = _sqlDataProvider.AddTestDrivenDNNModule(_mockModules[0], STR_Content, _mockUserIds[0]);
			//int itemId2 = _sqlDataProvider.AddTestDrivenDNNModule(_mockModules[0], STR_Content, _mockUserIds[0]);
			//Assert.IsTrue(itemId1 > 0);
			//Assert.IsTrue(itemId2 > 0);

			//using (IDataReader dataReader = _sqlDataProvider.GetTestDrivenDNNModules(_mockModules[0])) {
			//     while (dataReader.Read()) {
			//          var actualId = (int)dataReader["ItemId"];
			//          if (actualId == itemId1 || actualId == itemId2)
			//               records++;
			//     }
			//}

			//Assert.AreEqual<int>(2, records);
		}
	}
}
