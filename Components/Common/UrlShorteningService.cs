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
using System.IO;
using System.Net;
using System.Text;

namespace DotNetNuke.DNNQA.Components.Common
{
	/// <summary>
	/// Wrapper for interacting with TinyUrl API
	/// </summary>
	public class UrlShorteningService
	{

		#region Members

		private string requestTemplate;

		private string baseUrl;

		#endregion

		public UrlShorteningService(ShorteningService shorteningService__1, string account, string apiKey)
		{
			switch (shorteningService__1)
			{
				case ShorteningService.isgd:
					requestTemplate = "http://is.gd/api.php?longurl={0}";
					baseUrl = "is.gd";
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case ShorteningService.Bitly:
					//requestTemplate = "http://bit.ly/api?url={0}"
					requestTemplate = "http://api.bit.ly/v3/shorten?login=" + account + "&apiKey=" + apiKey + "&longUrl={0}&format=txt";
					baseUrl = "bit.ly";
					break; // TODO: might not be correct. Was : Exit Select

					break;
				case ShorteningService.Cligs:
					requestTemplate = "http://cli.gs/api/v1/cligs/create?url={0}&appid=WittyTwitter";
					baseUrl = "cli.gs";
					break; // TODO: might not be correct. Was : Exit Select

					break;
				default:
					requestTemplate = "http://tinyurl.com/api-create.php?url={0}";
					baseUrl = "tinyurl.com";
					break; // TODO: might not be correct. Was : Exit Select

					break;
			}
		}

		public string ShrinkUrls(string text)
		{
			return ShrinkUrls(text, null);
		}

		public string ShrinkUrls(string text, IWebProxy webProxy)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}

			var textSplitIntoWords = text.Split(' ');
			var foundUrl = false;

			for (var i = 0; i <= textSplitIntoWords.Length - 1; i++)
			{
				if (IsUrl(textSplitIntoWords[i]))
				{
					foundUrl = true;
					// replace found url with tinyurl
					textSplitIntoWords[i] = GetNewShortUrl(textSplitIntoWords[i], webProxy);
				}
			}

			// reassemble if we found at least 1 url, otherwise return unaltered
			return foundUrl ? String.Join(" ", textSplitIntoWords) : text;
		}

		/// <summary>
		/// This can definitely be refactored
		/// </summary>
		/// <param name="sourceUrl"></param>
		/// <returns></returns>
		public bool IsShortenedUrl(string sourceUrl)
		{
			return sourceUrl.Contains("http://tinyurl.com") || sourceUrl.Contains("http://bit.ly") || sourceUrl.Contains("http://is.gd") || sourceUrl.Contains("http://cli.gs");
		}

		public string GetNewShortUrl(string sourceUrl, IWebProxy webProxy)
		{
			if (sourceUrl == null)
			{
				throw new ArgumentNullException("sourceUrl");
			}

			// fallback will be source url
			string result = sourceUrl;
			//Added 11/3/2007 scottckoon
			//20 is the shortest a tinyURl can be (http://tinyurl.com/a)
			//so if the sourceUrl is shorter than that, don't make a request to TinyURL
			if (sourceUrl.Length > 20 && !IsShortenedUrl(sourceUrl))
			{
				// tinyurl doesn't like urls w/o protocols so we'll ensure we have at least http
				string requestUrl = string.Format(this.requestTemplate, (EnsureMinimalProtocol(sourceUrl)));
				WebRequest request = HttpWebRequest.Create(requestUrl);

				request.Proxy = webProxy;

				try
				{
					using (Stream responseStream = request.GetResponse().GetResponseStream())
					{
						StreamReader reader = new StreamReader(responseStream, Encoding.ASCII);
						result = reader.ReadToEnd();
					}
				}
				catch
				{
					// eat it and return original url
				}
			}
			//scottckoon - It doesn't make sense to return a TinyURL that is longer than the original.
			if (result.Length > sourceUrl.Length)
			{
				result = sourceUrl;
			}
			return result;
		}

		public static bool IsUrl(string word)
		{
			if (!Uri.IsWellFormedUriString(word, UriKind.Absolute))
			{
				return false;
			}

			Uri uri__1 = new Uri(word);
			foreach (string acceptedScheme in new string[] {
				"http",
				"https",
				"ftp"
			})
			{
				if (uri__1.Scheme == acceptedScheme)
				{
					return true;
				}
			}

			return false;
		}

		private static string EnsureMinimalProtocol(string url)
		{
			// if our url doesn't have a protocol, we'll at least assume it's plain old http, otherwise good to go
			const string minimalProtocal = "http://";
			if (url.ToLower().StartsWith("http"))
			{
				return url;
			}
			else
			{
				return minimalProtocal + url;
			}
		}

	}

	public enum ShorteningService
	{
		TinyUrl,
		Bitly,
		isgd,
		Cligs
	}

}
