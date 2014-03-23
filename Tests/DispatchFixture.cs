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
using System.Collections.Specialized;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Tests.Mocks;
using DotNetNuke.Services.Cache;
using MbUnit.Framework;
using Moq;
using System.Web;

namespace DotNetNuke.DNNQA.Tests 
{

	public class DispatchFixture {

		#region Object being tested

		private DispatchPresenter _presenter;

		#endregion

		#region Mocked objects

		private Mock<CachingProvider> _mockCache;
		private Mock<IDispatchView> _view;
		private Mock<IDnnqaController> _controller;
		private int _controlViewID;

		#endregion

		#region Setup for each test

		/// <summary>
		/// Setup performs common Arrange work used by the tests.
		/// </summary>
		[SetUp]
		public void Setup() {
			// Arrange...
			_controlViewID = 0;
			//      the View
			_view = new Mock<IDispatchView>();
			_view.SetupAllProperties();
			//      the Controller
			_controller = new Mock<IDnnqaController>(MockBehavior.Strict);
			//      the background caching
			_mockCache = MockCachingProvider.CreateMockProvider();
			CreatePresenter();
		}

		#endregion

		#region Helpers

		#region - for Presenter

		public void CreatePresenter(Mock<IDispatchView> mockedView, Mock<IDnnqaController> mockedController, Mock<HttpContextBase> mockedHttpContext, int controlViewID) {
			_presenter = new DispatchPresenter(mockedView.Object, mockedController.Object)
			{
				HttpContext = mockedHttpContext.Object,
				//ModuleId = data.ModuleId,
				//UserId = data.CreatedByUser,
				TabId = controlViewID
			};

		}

		private void CreatePresenter() {
			var requestParams = new NameValueCollection();
			requestParams.Add("view", "1");
			CreatePresenter(_view, _controller, CreateHttpContext(requestParams), _controlViewID);
		}

		public void CreatePresenter(Mock<HttpContextBase> mockedHttpContext) {
			CreatePresenter(_view, _controller, mockedHttpContext, _controlViewID);
		}

		#endregion

		#region - for HttpContextBase

		private Mock<HttpContextBase> CreateHttpContext(NameValueCollection requestParams) {
			return CreateHttpContext(new Mock<HttpRequestBase>(), new Mock<HttpResponseBase>(), requestParams);
		}

		private Mock<HttpContextBase> CreateHttpContext(Mock<HttpResponseBase> mockedHttpResponse) {
			return CreateHttpContext(new Mock<HttpRequestBase>(), mockedHttpResponse, null);
		}

		private Mock<HttpContextBase> CreateHttpContext(Mock<HttpResponseBase> mockedHttpResponse, NameValueCollection requestParams) {
			return CreateHttpContext(new Mock<HttpRequestBase>(), mockedHttpResponse, requestParams);
		}

		private static Mock<HttpContextBase> CreateHttpContext(Mock<HttpRequestBase> mockedHttpRequest, Mock<HttpResponseBase> mockedHttpResponse, NameValueCollection requestParams) {
			mockedHttpRequest.Setup(self => self.Params).Returns(requestParams);
			var mockHttpContext = new Mock<HttpContextBase>();
			mockHttpContext.Setup(self => self.Response).Returns(mockedHttpResponse.Object);
			mockHttpContext.Setup(self => self.Request).Returns(mockedHttpRequest.Object);
			return mockHttpContext;
		}

		#endregion

		#endregion

		#region Test Constructor

		/// <summary>
		/// 
		/// </summary>
		[Test]
		[Description("Ensure that the constructor checks for a non-null Controller object")]
		[Annotation(Gallio.Model.AnnotationType.Info, "Owner: Unclaimed")]
		public void Presenter_Constructor_Requires_Non_Null_Controller() {
			// Arrange (see the SetUp method)
			// Act
			// Assert
			Assert.Throws<ArgumentNullException>(() => { _presenter = new DispatchPresenter(_view.Object, null); });
		}

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// Note that it is impossible for presenters to perform their own validation
		/// of the View before calling the base class constructor; the base constructor
		/// does not perform validation for a null View, but the base's constructor uses
		/// the View to set its Model. Attempting to use a non-null view will result in 
		/// a NullReferenceException.
		/// </remarks>
		[Test]
		[Description("Ensure that the constructor checks for a non-null View object")]
		[Annotation(Gallio.Model.AnnotationType.Info, "Owner: Unclaimed")]
		public void Presenter_Constructor_Requires_Non_Null_View() {
			// Arrange (see the SetUp method)
			// Act
			// Assert
			Assert.Throws<NullReferenceException>(() => { _presenter = new DispatchPresenter(null); });
			Assert.Throws<NullReferenceException>(() => { _presenter = new DispatchPresenter(null, _controller.Object); });
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		[Description("Ensure that the constructor calls the base class, which sets up the View's Model property")]
		[Annotation(Gallio.Model.AnnotationType.Info, "Owner: Unclaimed")]
		public void Presenter_Constructor_Sets_View_Model_Instance() {
			// Arrange (see the SetUp method)
			// Act
			_presenter = new DispatchPresenter(_view.Object, _controller.Object);

			// Assert
			Assert.IsNotNull(_presenter.View.Model, "Model was not assigned by the Presenter to the View");
		}

		#endregion

		#region Test Control Loading


		#endregion

	}
}
