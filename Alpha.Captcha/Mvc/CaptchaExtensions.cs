namespace Alpha.Captcha.Mvc
{
    using Alpha.Captcha.Helpers;
    using Alpha.Captcha.Models;
    using Alpha.Captcha.Resources;
    using System.Text;
    using System.Web.Mvc;

    public static class CaptchaExtensions
    {
        public static MvcHtmlString Captcha(this HtmlHelper helper, CaptchaLevels level)
        {
            return Captcha(helper, level, Configuration.CaptchaInputName);
        }

        public static MvcHtmlString Captcha(this HtmlHelper helper, CaptchaLevels level, string inputName)
        {
            CreateCaptchaSession(helper, level);
            return MvcHtmlString.Create(CreateCaptchaHtml(helper, inputName));
        }

        private static void CreateCaptchaSession(this HtmlHelper helper, CaptchaLevels level)
        {
            var session = helper.ViewContext.HttpContext.Session;
            session[Configuration.CaptchaSessionLevel] = level;
        }

        private static string CreateCaptchaHtml(this HtmlHelper helper, string inputName)
        {
            var html = new StringBuilder();
            var container = new TagBuilder("div");
            container.MergeAttribute("class", "captcha-container");
            html.Append(container.ToString(TagRenderMode.StartTag));
            //html.Append(CreateInstruction(helper));
            html.Append(CreateCaptchaImage(helper));
            html.Append(CreateCaptchaInput(helper, inputName));
            html.Append(container.ToString(TagRenderMode.EndTag));
            html.Append(CreateCaptchaScript(helper));
            return html.ToString();
        }

        private static string CreateCaptchaInput(HtmlHelper helper, string inputName)
        {
            var html = new StringBuilder();
            var row = CreateRowTag(helper, "row-input");

            var inputBuilder = new TagBuilder("input");
            inputBuilder.MergeAttribute("class", "captcha-input");
            inputBuilder.MergeAttribute("name", inputName);
            //inputBuilder.MergeAttribute("placeholder", Configuration.CaptchaInputPlaceholder);
            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append(inputBuilder.ToString(TagRenderMode.SelfClosing));
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateCaptchaImage(HtmlHelper helper)
        {
            var html = new StringBuilder();
            var row = CreateRowTag(helper, "row-image");

            var imageUrl = CreateActionUrl(helper, "Image", false);
            var imageBuilder = new TagBuilder("img");
            imageBuilder.MergeAttribute("class", "captcha-image");
            imageBuilder.MergeAttribute("src", imageUrl);

            var refreshIconUrl = CreateActionUrl(helper, "RefreshIcon");
            var refreshIconBuilder = new TagBuilder("img");
            refreshIconBuilder.MergeAttribute("class", "refresh-icon");
            refreshIconBuilder.MergeAttribute("src", refreshIconUrl);

            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append(imageBuilder.ToString(TagRenderMode.SelfClosing));
            html.Append(refreshIconBuilder.ToString(TagRenderMode.SelfClosing));
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateInstruction(HtmlHelper helper)
        {
            var html = new StringBuilder();
            var row = CreateRowTag(helper, "row-instruction");
            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append($"<div class='captcha-instruction'>{Lang.IamNotARobot}</div>");
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateCaptchaScript(HtmlHelper helper)
        {
            var url = CreateActionUrl(helper, "Script");
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.MergeAttribute("src", url);
            scriptBuilder.MergeAttribute("type", "text/javascript");
            return $"{scriptBuilder.ToString(TagRenderMode.StartTag)}{scriptBuilder.ToString(TagRenderMode.EndTag)}";
        }

        private static TagBuilder CreateRowTag(HtmlHelper helper, string customClass)
        {
            var rowBuilder = new TagBuilder("div");
            rowBuilder.MergeAttribute("class", $"captcha-row {customClass}");
            return rowBuilder;
        }

        private static string CreateActionUrl(HtmlHelper helper, string action, bool includeVersion = true)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var url = urlHelper.Action(action, "Captcha");
            return includeVersion ? $"{url}?v={AssemblyHelpers.Version}" : url;
        }
    }
}