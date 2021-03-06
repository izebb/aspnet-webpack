﻿using System.Linq;
using System.Text;
using Webpack.Extensions;

namespace Webpack {
	internal class ArgumentsHelper {

		private const string DefaultDevFile = "--config webpack/webpack.dev.js ";
		private const string DevToolType = "--devtool {0} ";
		private const string CssFiles = "--module-bind css=style!css ";
		private const string LessFiles = "--module-bind less=style!css!less ";
		private const string SassFiles = "--module-bind scss=style!css!sass ";
		private const string HandleAngularTemplateFiles = "--module-bind html=raw ";
		private const string StaticFile = "--module-bind {0}=url?limit={1} ";

		/// <summary>
		/// Creates and returns the appropriate arguments list for the webpack based on the provided options
		/// </summary>
		public static string GetWebpackArguments(string rootPath, WebpackOptions options, bool includeDefaultConfigFile) {
			var result = new StringBuilder();
			if(includeDefaultConfigFile) {
				result.Append(DefaultDevFile);
			}
			if (options.HandleStyles && options.StylesTypes.Any()) {
				if (options.StylesTypes.Contains(StylesType.Css)) {
					result.Append(CssFiles);
				}
				if (options.StylesTypes.Contains(StylesType.Sass)) {
					result.Append(SassFiles);
				}
				if (options.StylesTypes.Contains(StylesType.Less)) {
					result.Append(LessFiles);
				}
			}
			if(options.HandleAngularTemplates) {
				result.Append(HandleAngularTemplateFiles);
			}
			if(options.HandleStaticFiles) {
				options.StaticFileTypes.ToList().ForEach(staticFileType => {
					result.Append(string.Format(StaticFile, staticFileType.ToString().ToLowerInvariant(), options.StaticFileTypesLimit));
				});
			}
			result.Append($"--entry ./{options.EntryPoint} ");
			result.Append($"--output-path {rootPath} ");
			result.Append($"--output-filename {options.OutputFileName} ");
			result.Append(string.Format(DevToolType, options.DevToolType.GetWebpackValue()));

			if(options.EnableHotLoading) {
				result.Append("--hot --inline ");
				result.Append($"--host {options.DevServerOptions.Host} ");
				result.Append($"--port {options.DevServerOptions.Port} ");
				result.Append($"--output-public-path http://{options.DevServerOptions.Host}:{options.DevServerOptions.Port}/ ");
			}

			return result.ToString();
		}
	}
}
