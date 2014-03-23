//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2012
// by DotNetNuke Corporation
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
using DotNetNuke.ComponentModel;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Web.Mvp;

namespace DotNetNuke.DNNQA.Components.Presenters
{

    /// <summary>
    /// 
    /// </summary>
    public class QaServicePresenter : WebServicePresenter<IQaServiceView>
    {

        #region Private Members

        protected IDnnqaController Controller { get; private set; }

        #endregion

        #region Constructors

        public QaServicePresenter(IQaServiceView view)
            : this(view, GetQaController(GetRepository()))
        {
        }

        public QaServicePresenter(IQaServiceView view, IDnnqaController controller)
            : base(view)
        {
            if (view == null)
            {
                throw new ArgumentException(@"View is nothing.", "view");
            }

            if (controller == null)
            {
                throw new ArgumentException(@"Controller is nothing.", "controller");
            }

            Controller = controller;
            View.ListQuestionTitleCalled += SearchQuestionTitle;
        }

        #endregion

        #region Private Methods

        private void SearchQuestionTitle(object sender, SearchQuestionTitleEventArgs e)
        {
            e.Result = Controller.SearchQuestionTitles(e.ModuleId, e.SearchPhrase);
        }

        private static IDataProvider GetRepository()
        {
            var ctl = ComponentFactory.GetComponent<IDataProvider>();

            if (ctl == null)
            {
                ctl = new SqlDataProvider();
                ComponentFactory.RegisterComponentInstance<IDataProvider>(ctl);
            }
            return ctl;
        }

        private static IDnnqaController GetQaController(IDataProvider repository)
        {
            var ctl = ComponentFactory.GetComponent<IDnnqaController>();

            if (ctl == null)
            {
                ctl = new DnnqaController(repository);
                ComponentFactory.RegisterComponentInstance<IDnnqaController>(ctl);
            }
            return ctl;
        }

        #endregion

    }
}