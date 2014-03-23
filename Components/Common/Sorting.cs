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

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.DNNQA.Components.Entities;

namespace DotNetNuke.DNNQA.Components.Common
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Do NOT use WHERE clauses in here</remarks>
    public class Sorting
    {

        internal static IEnumerable<TermInfo> GetTermCollection(int pageSize, int pageIndex, SortInfo objSorting, IEnumerable<TermInfo> resultsCollection)
        {
            var defaultResults = resultsCollection.Skip(pageSize * pageIndex).Take(pageSize).ToList();

            if (objSorting != null)
                switch (objSorting.Column)
                {
                    case "name":
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.Name descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.Name ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                    case "popular":
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.TotalTermUsage descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.TotalTermUsage ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                    case "newest":
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.CreatedOnDate descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.CreatedOnDate ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                    default: // "daily";
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.DayTermUsage descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.DayTermUsage ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                }
            return defaultResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="tagMode"></param>
        /// <param name="resultsCollection"></param>
        /// <returns></returns>
        internal static IEnumerable<TermInfo> GetHomeTermCollection(int pageSize, Constants.TagMode tagMode, IEnumerable<TermInfo> resultsCollection)
        {
            switch (tagMode)
            {
                case Constants.TagMode.ShowDailyUsage:
                    return (from t in resultsCollection orderby t.DayTermUsage descending, t.Name ascending select t).Skip(0).Take(pageSize);
                case Constants.TagMode.ShowWeeklyUsage:
                    return (from t in resultsCollection orderby t.WeekTermUsage descending, t.Name ascending select t).Skip(0).Take(pageSize);
                case Constants.TagMode.ShowMonthlyUsage:
                    return (from t in resultsCollection orderby t.MonthTermUsage descending, t.Name ascending select t).Skip(0).Take(pageSize);
                default: // total
                    return (from t in resultsCollection orderby t.TotalTermUsage descending, t.Name ascending select t).Skip(0).Take(pageSize);
            }
        }

        internal static IEnumerable<PostInfo> GetAnswerCollection(int pageSize, int pageIndex, SortInfo objSorting, IEnumerable<PostInfo> resultsCollection)
        {
            var defaultResults = resultsCollection.Skip(pageSize * pageIndex).Take(pageSize).ToList();

            if (objSorting != null)
                switch (objSorting.Column)
                {
                    case "oldest":
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.AnswerId descending, t.CreatedDate descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.AnswerId descending, t.CreatedDate ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                    case "active":
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.AnswerId descending, t.LastModifiedDate descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.AnswerId descending, t.LastModifiedDate ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                    default: // "votes";
                        switch (objSorting.Direction)
                        {
                                // we always want the accepted answer first, then number of votes, and finally if 2+ answers have same number of votes, we want the newest one on top.
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.AnswerId descending, t.Score descending, t.CreatedDate descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.AnswerId descending, t.Score ascending, t.CreatedDate descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                }
            return defaultResults;
        }

        internal static IEnumerable<QuestionInfo> GetKeywordSearchCollection(int pageSize, int pageIndex, SortInfo objSorting, IEnumerable<QuestionInfo> resultsCollection)
        {
            var defaultResults = resultsCollection.Skip(pageSize * pageIndex).Take(pageSize).ToList();

            if (objSorting != null)
                switch (objSorting.Column)
                {
                    case "newest":
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.CreatedOnDate descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.CreatedOnDate ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                    case "votes":
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.Score descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.Score ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                    default: // "active";
                        switch (objSorting.Direction)
                        {
                            case Constants.SortDirection.Descending:
                                return (from t in resultsCollection orderby t.LastApprovedDate descending select t).Skip(pageSize * pageIndex).Take(pageSize);
                            default:
                                return (from t in resultsCollection orderby t.LastApprovedDate ascending select t).Skip(pageSize * pageIndex).Take(pageSize);
                        }
                }
            return defaultResults;
        }

        internal static IEnumerable<UserScoreLogInfo> GetUserRepCollection(int pageSize, IEnumerable<UserScoreLogInfo> resultsCollection)
        {
            return (from t in resultsCollection where t.Score != 0 orderby t.CreatedOnDate descending select t).Skip(0).Take(pageSize);
        }

    }
}