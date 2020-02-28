namespace Alpha.Captcha.Mvc
{
    using Alpha.Captcha.Helpers;
    using System.Text;
    using System.Web.Mvc;

    public static class CaptchaExtensions
    {
        public static MvcHtmlString Captcha(this HtmlHelper helper)
        {
            return Captcha(helper, Configuration.CaptchaInputName);
        }

        public static MvcHtmlString Captcha(this HtmlHelper helper, string inputName)
        {
            var html = new StringBuilder();
            var container = new TagBuilder("div");
            container.MergeAttribute("class", "captcha-container");
            html.Append(container.ToString(TagRenderMode.StartTag));
            html.Append(CreateCaptchaImage(helper));
            html.Append(CreateInstruction(helper));
            html.Append(CreateCaptchaInput(helper, inputName));
            html.Append(container.ToString(TagRenderMode.EndTag));
            html.Append(CreateCaptchaScript(helper));
            return MvcHtmlString.Create(html.ToString());
        }

        private static string CreateCaptchaInput(HtmlHelper helper, string inputName)
        {
            var inputBuilder = new TagBuilder("input");
            inputBuilder.MergeAttribute("name", inputName);
            return $"<div class='captcha-input'>{inputBuilder.ToString(TagRenderMode.SelfClosing)}<a href='#'>{Configuration.CaptchaRefresh}</a></div>";
        }

        private static string CreateCaptchaImage(HtmlHelper helper)
        {
            var imagePlaceholder = new TagBuilder("div");
            imagePlaceholder.MergeAttribute("class", "captcha-image");
            imagePlaceholder.MergeAttribute("style", "width: 200px; height: 50px");
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            string url = urlHelper.Action("Image", "Captcha");
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url);
            return $"{imagePlaceholder.ToString(TagRenderMode.StartTag)}{imgBuilder.ToString(TagRenderMode.SelfClosing)}{imagePlaceholder.ToString(TagRenderMode.EndTag)}";
        }

        private static string CreateInstruction(HtmlHelper helper)
        {
            return $"<div>{Configuration.CaptchaInstruction}</div>";
        }

        private static string CreateCaptchaScript(HtmlHelper helper)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            string url = $"{urlHelper.Action("Script", "Captcha")}?v={AssemblyHelpers.Version}";
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.MergeAttribute("src", url);
            scriptBuilder.MergeAttribute("type", "text/javascript");
            return $"{scriptBuilder.ToString(TagRenderMode.StartTag)}{scriptBuilder.ToString(TagRenderMode.EndTag)}";
        }
    }
}