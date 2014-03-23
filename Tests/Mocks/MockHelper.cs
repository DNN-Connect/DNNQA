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

using System.Data;

namespace DotNetNuke.DNNQA.Tests.Mocks
{

    public static class MockHelper
    {
        public const int ModuleId = 381;
        public const int ValidModuleId = 1;
        public const int LoadExceptionId = -2;
        public const int AddExceptionId = -3;
        public const int UpdateExceptionId = -4;
        public const int DeleteExceptionId = -5;

	   //public const string content = "Content";
	   //public const string updateContent = "Updated Content";
	   //public const string createdByUserName = "User 1";
	   //public const int createdByUser = 1;
	   //public const int createdDateYear = 2008;
	   //public const int createdDateMonth = 1;
	   //public const int createdDateDay = 1;

	   //public static ProjectInfo CreateTestDrivenDNNModuleInfo()
	   //{

	   //     ProjectInfo testDrivenDNNModuleInfo = new ProjectInfo()
	   //    {
	   //        Content = MockHelper.content,
	   //        CreatedByUser = MockHelper.createdByUser,
	   //        CreatedByUserName = MockHelper.createdByUserName,
	   //        CreatedDate = new DateTime(MockHelper.createdDateYear, MockHelper.createdDateMonth, MockHelper.createdDateDay),
	   //        ItemId = 1,
	   //        ModuleId = ModuleId
	   //    }; 
	   //    return testDrivenDNNModuleInfo;
	   //}

        public static IDataReader CreateTestDrivenModuleDataReader()
        {
		  //DataTable datatable = new DataTable();
		  //datatable.Columns.Add("ModuleId");
		  //datatable.Columns.Add("ItemId");
		  //datatable.Columns.Add("Content");
		  //datatable.Columns.Add("CreatedByUser");
		  //datatable.Columns.Add("CreatedDate");
		  //datatable.Columns.Add("CreatedByUserName");

		  //datatable.Rows.Add(ModuleId, ValidTestDrivenDNNModuleInfoId, content, createdByUser, new DateTime(createdDateYear, createdDateMonth, createdDateDay), createdByUserName);

		  //return datatable.CreateDataReader();
        	return null;
        }

        public static IDataReader CreateTestDrivenModuleDataReader2Rows()
        {
		  //DataTable datatable = new DataTable();
		  //datatable.Columns.Add("ModuleId");
		  //datatable.Columns.Add("ItemId");
		  //datatable.Columns.Add("Content");
		  //datatable.Columns.Add("CreatedByUser");
		  //datatable.Columns.Add("CreatedDate");
		  //datatable.Columns.Add("CreatedByUserName");

		  //datatable.Rows.Add(ModuleId, ValidTestDrivenDNNModuleInfoId, content, createdByUser, new DateTime(createdDateYear, createdDateMonth, createdDateDay), createdByUserName);
		  //datatable.Rows.Add(ModuleId, ValidTestDrivenDNNModuleInfoId, content, createdByUser, new DateTime(createdDateYear, createdDateMonth, createdDateDay), createdByUserName);

		  //return datatable.CreateDataReader();
        	return null;
        }

    }
}
