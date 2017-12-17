using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Fabricam.Web.Filters
{

	[HtmlTargetElement(Attributes = "auth-show")]
	public class AuthShowTagHelper : TagHelper
    {
		[ViewContext] // Current context injection
		[HtmlAttributeNotBound] // don't let users change this
		public ViewContext ViewContext { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
        {
			bool isAuthed = this.ViewContext.HttpContext.User?.Identity.IsAuthenticated ?? false;
			if (!isAuthed) {
				output.SuppressOutput();
			}
		}
	}

	[HtmlTargetElement(Attributes = "auth-hide")]
	public class AuthHideTagHelper : TagHelper
    {
		[ViewContext] // Current context injection
		[HtmlAttributeNotBound] // don't let users change this
		public ViewContext ViewContext { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            bool isAuthed = this.ViewContext.HttpContext.User?.Identity.IsAuthenticated ?? false;
            if (isAuthed) {
				output.SuppressOutput();
			}
		}
	}
	
}
