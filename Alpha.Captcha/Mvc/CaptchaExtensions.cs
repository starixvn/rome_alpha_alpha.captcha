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
            return Captcha(helper, new CaptchaSetting(level));
        }

        public static MvcHtmlString Captcha(this HtmlHelper helper, CaptchaSetting setting)
        {
            CreateCaptchaSession(helper, setting);
            return MvcHtmlString.Create(CreateCaptchaHtml(helper, setting));
        }

        private static void CreateCaptchaSession(this HtmlHelper helper, CaptchaSetting setting)
        {
            var session = helper.ViewContext.HttpContext.Session;
            session[Configuration.CaptchaSessionLevel] = setting.Level;
        }

        private static string CreateCaptchaHtml(this HtmlHelper helper, CaptchaSetting setting)
        {
            var html = new StringBuilder();
            var tagName = setting.IsForm ? "form" : "div";
            var container = new TagBuilder(tagName);
            container.MergeAttribute("class", "captcha-container");
            if (setting.IsForm)
            {
                container.MergeAttribute("method", "post");
            }

            html.Append(container.ToString(TagRenderMode.StartTag));
            html.Append(CreateHint(helper));
            html.Append(CreateCaptchaImage(helper));
            html.Append(CreateCaptchaInput(helper, setting.InputName));
            if (setting.IsShowButton)
            {
                html.Append(CreateButton(helper, setting));
            }

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
            //inputBuilder.MergeAttribute("placeholder", Lang.InputPlaceholder);
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

            var refreshIconBuilder = new TagBuilder("i");
            refreshIconBuilder.MergeAttribute("class", "icon-refresh");
            refreshIconBuilder.MergeAttribute("title", Lang.RefreshTitle);

            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append(imageBuilder.ToString(TagRenderMode.SelfClosing));
            html.Append(refreshIconBuilder.ToString(TagRenderMode.StartTag));
            html.Append(refreshIconBuilder.ToString(TagRenderMode.EndTag));
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateHint(HtmlHelper helper)
        {
            var html = new StringBuilder();
            var row = CreateRowTag(helper, "row-hint");
            html.Append(row.ToString(TagRenderMode.StartTag));
            html.Append("<i class='icon-check-circle'></i>");
            html.Append($"<span class='captcha-hint'>{Lang.IamNotARobot}</span>");
            html.Append(row.ToString(TagRenderMode.EndTag));
            return html.ToString();
        }

        private static string CreateButton(HtmlHelper helper, CaptchaSetting setting)
        {
            var html = new StringBuilder();
            var tag = new TagBuilder("button");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("class", "captcha-button " + setting.ButtonClassname);
            html.Append(tag.ToString(TagRenderMode.StartTag));
            html.Append(Lang.ButtonText);
            html.Append(tag.ToString(TagRenderMode.EndTag));
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