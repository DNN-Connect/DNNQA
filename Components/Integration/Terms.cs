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

using System.Linq;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Content;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Entities.Content.Taxonomy;

namespace DotNetNuke.DNNQA.Components.Integration {

	public class Terms {

		/// <summary>
		/// This should run only after the post has been added/updated in data store and the ContentItem exists.
		/// </summary>
		/// <param name="objPost">The content item we are associating categories with. In this module, it will always be a question (first post).</param>
		/// <param name="objContent"></param>
		internal void ManageQuestionTerms(PostInfo objPost, ContentItem objContent)
		{
			RemoveQuestionTerms(objContent);

			foreach (var term in objPost.Terms) {
				Util.GetTermController().AddTermToContent(term, objContent);
			}
		}

		/// <summary>
		/// Removes terms associated w/ a specific ContentItem.
		/// </summary>
		/// <param name="objContent"></param>
		internal void RemoveQuestionTerms(ContentItem objContent)
		{
			Util.GetTermController().RemoveTermsFromContent(objContent);
		}

		/// <summary>
		/// This method will check the core taxonomy to ensure that a term exists, if not it will create.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="vocabularyId"></param>
		/// <returns>The core 'Term'.</returns>
		internal static Term CreateAndReturnTerm(string name, int vocabularyId)
		{
			var termController = Util.GetTermController();
			var existantTerm = termController.GetTermsByVocabulary(vocabularyId).Where(t => t.Name.ToLower() == name.ToLower()).FirstOrDefault();
			if (existantTerm != null)
			{
				return existantTerm;
			}

			var termId = termController.AddTerm(new Term(vocabularyId) { Name = name });
			return new Term { Name = name, TermId = termId };
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="vocabularyId"></param>
		/// <returns>The core 'Term'.</returns>
		internal static Term GetTermById(int id, int vocabularyId)
		{
			var termController = Util.GetTermController();
			var existantTerm = termController.GetTermsByVocabulary(vocabularyId).Where(t => t.TermId == id).FirstOrDefault();
			return existantTerm;
		}

	}
}