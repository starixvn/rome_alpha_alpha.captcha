namespace Alpha.Captcha.Mvc
{
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Alpha.Captcha.Helpers;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Resources;
    using Alpha.Captcha.Services;

    public static class CaptchaExtensions
    {
        public static MvcHtmlString Captcha(this HtmlHelper helper, CaptchaSetting captchaSetting)
        {
            var sessionService = DependencyResolver.Current.GetService(typeof(ISessionService)) as ISessionService ?? new SessionService();
            sessionService.Create(captchaSetting);
            return MvcHtmlString.Create(CreateHtml(helper, captchaSetting));
        }

        public static MvcHtmlString Stylesheet(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(CreateStylesheetHtml());
        }

        private static string CreateHtml(this HtmlHelper helper, CaptchaSetting captchaSetting)
        {
            var html = new StringBuilder();
            var tagName = captchaSetting.IsForm ? "form" : "div";
            var container = new TagBuilder(tagName);
            container.MergeAttribute("class", "captcha-container");
            if (captchaSetting.IsForm)
            {
                container.MergeAttribute("method", "post");
            }

            html.Append(container.ToString(TagRenderMode.StartTag));
            if (!string.IsNullOrWhiteSpace(captchaSetting.FunctionName))
            {
                html.Append($"<input name='{Configuration.CaptchaFunctionName}' value='{captchaSetting.FunctionName}' hidden />");
            }

            html.Append(CreateHintHtml());
            html.Append(CreateImageHtml(captchaSetting.FunctionName));
            html.Append(CreateInputHtml());
            if (captchaSetting.IsShowButton)
            {
                html.Append(CreateButtonHtml(captchaSetting.ButtonClassname));
            }

            html.Append(container.ToString(TagRenderMode.EndTag));
            html.Append(CreateScriptHtml());
            return html.ToString();
        }

        private static string CreateInputHtml()
        {
            var html = new StringBuilder();
            var row = CreateRowTag("row-input");

            var inputBuilder = new TagBuilder("input");
            inputBuilder.MergeAttribute("class", "captcha-input");
            inputBuilder.MergeAttribute("name", Configuration.CaptchaInputName);
            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append(inputBuilder.ToString(TagRenderMode.SelfClosing));
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateImageHtml(string functionName)
        {
            var html = new StringBuilder();
            var row = CreateRowTag("row-image");

            var imageUrl = CreateActionUrl("Image", false);
            if (!string.IsNullOrEmpty(functionName))
            {
                imageUrl += $"?{Configuration.CaptchaFunctionName}={functionName.ToLowerInvariant()}";
            }

            var imageBuilder = new TagBuilder("img");
            imageBuilder.MergeAttribute("class", "captcha-image");
            imageBuilder.MergeAttribute("src", imageUrl);

            var refreshIconBuilder = new TagBuilder("i");
            refreshIconBuilder.MergeAttribute("class", "captcha-icon-refresh");
            refreshIconBuilder.MergeAttribute("title", Lang.RefreshTitle);

            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append(imageBuilder.ToString(TagRenderMode.SelfClosing));
            html.Append(refreshIconBuilder.ToString(TagRenderMode.StartTag));
            html.Append(refreshIconBuilder.ToString(TagRenderMode.EndTag));
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateHintHtml()
        {
            var html = new StringBuilder();
            var row = CreateRowTag("row-hint");
            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append("<i class='captcha-icon-check-circle'></i>");
            html.Append($"<span class='captcha-hint'>{Lang.IamNotARobot}</span>");
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateButtonHtml(string classname)
        {
            var html = new StringBuilder();
            var tag = new TagBuilder("button");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("class", "captcha-button " + classname);
            html.Append(tag.ToString(TagRenderMode.StartTag));
            html.Append(Lang.ButtonText);
            html.Append(tag.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateScriptHtml()
        {
            var url = CreateActionUrl("Script", true);
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.MergeAttribute("src", url);
            scriptBuilder.MergeAttribute("type", "text/javascript");
            return $"{scriptBuilder.ToString(TagRenderMode.StartTag)}{scriptBuilder.ToString(TagRenderMode.EndTag)}";
        }

        private static string CreateStylesheetHtml()
        {
            var url = CreateActionUrl("Stylesheet", true);
            var scriptBuilder = new TagBuilder("link");
            scriptBuilder.MergeAttribute("href", url);
            scriptBuilder.MergeAttribute("rel", "stylesheet");
            return $"{scriptBuilder.ToString(TagRenderMode.StartTag)}{scriptBuilder.ToString(TagRenderMode.EndTag)}";
        }

        private static TagBuilder CreateRowTag(string customClass)
        {
            var rowBuilder = new TagBuilder("div");
            rowBuilder.MergeAttribute("class", $"captcha-row {customClass}");
            return rowBuilder;
        }

        private static string CreateActionUrl(string action, bool includeVersion)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var url = urlHelper.Action(action, "Captcha");
            return includeVersion ? $"{url}?v={AssemblyHelpers.Version}" : url;
        }
    }
}