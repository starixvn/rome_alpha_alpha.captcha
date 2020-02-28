$(function () {
    var captchaImagePlacehoder = $('.captcha-image');
    var captchaRefreshIcon = $('.captcha-input a');
    var captchaInput = $('.captcha-input input');
    var registerImageEvent = function () {
        var captchaImage = captchaImagePlacehoder.find('img');
        captchaImage.load(function () {
            captchaInput.focus();
        });
    };
    var freshCaptcha = function () {
        captchaInput.val('');
        var captchaImageHtml = captchaImagePlacehoder.html();
        captchaImagePlacehoder.html('');
        captchaImagePlacehoder.append(captchaImageHtml);
        registerImageEvent();
    };
    captchaRefreshIcon.click(freshCaptcha);
});
