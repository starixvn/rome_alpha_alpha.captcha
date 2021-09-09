namespace Alpha.Captcha.Models
{
    public class SessionKey
    {
        public SessionKey(string setting, string text)
        {
            Setting = setting;
            Text = text;
        }

        public string Setting { get; }

        public string Text { get; }
    }
}
