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
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Tests.Mocks;
using DotNetNuke.Services.Cache;
using MbUnit.Framework;
using Moq;

namespace DotNetNuke.DNNQA.Tests {
	public class QuestionsFixture {

		#region Object being tested

		private HomePresenter _presenter;

		#endregion

		#region Setup and Mocked objects

		#region Mocked objects

		private Mock<CachingProvider> _mockCache;
		private int _moduleID;
		private int _pageSize;
		private Dictionary<string, string> _settings;
		private Mock<IHomeView> _view;
		private Mock<IDnnqaController> _controller;
		private List<QuestionInfo> _data;

		#endregion

		#region Setup for each test
		[SetUp]
		public void Setup() {
			// Arrange...
			//      the module's data
			_data = new List<QuestionInfo>();
			string[] expectedContent = { "First one", "Second one", "Last one" };
			for (int i = 0; i < expectedContent.Length; i++) {
				//_data.Add(new QuestionInfo() { ModuleId = _moduleID, ItemId = i + 100, Content = expectedContent[i] });
			}
			//      the module's properties and settings
			_moduleID = 5;
			_pageSize = 20;
			_settings = new Dictionary<string, string>();
			_settings.Add("template", "");
			//      the View
			_view = new Mock<IHomeView>();
			_view.SetupAllProperties();
			//      the Controller
			_controller = new Mock<IDnnqaController>();
			_controller.Setup(self => self.GetHomeQuestions(_moduleID, _pageSize, Constants.DefaultOpQuestionFlagHomeRemoveCount, Constants.DefaultOpHomeQuestionMinScore)).Returns(_data);
			//      the background caching
			_mockCache = MockCachingProvider.CreateMockProvider();
			CreatePresenter();
		}
		#endregion

		#region Helper Methods

		private void CreatePresenter() {
			_presenter = new HomePresenter(_view.Object, _controller.Object) { Settings = _settings };
		}

		private void CreatePresenter(List<QuestionInfo> data) {
			_data = data;
			_controller = new Mock<IDnnqaController>();
			_controller.Setup(self => self.GetHomeQuestions(_moduleID, _pageSize, Constants.DefaultOpQuestionFlagHomeRemoveCount, Constants.DefaultOpHomeQuestionMinScore)).Returns(_data);

			CreatePresenter();
		}

		#endregion

		#endregion

	}
}
