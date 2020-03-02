$(function () {
    var captchaImageRow = $('.row-image');
    var captchaRefreshIcon = $('.refresh-icon');
    var captchaInput = $('.captcha-input');
    var registerImageEvent = function () {
        var captchaImage = $('.captcha-image');
        captchaImage.load(function () {
            captchaInput.focus();
        });
    };
    var freshCaptcha = function () {
        captchaInput.val('');
        var newCaptchaImage = $('<div>').append($('.captcha-image').clone()).html();
        $('.captcha-image').remove();
        captchaImageRow.prepend(newCaptchaImage);
        registerImageEvent();
    };
    captchaRefreshIcon.click(freshCaptcha);
});