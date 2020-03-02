document.addEventListener("DOMContentLoaded", function () {
    var captchaImage = document.getElementsByClassName('captcha-image')[0];
    var captchaImageSrc = captchaImage.getAttribute('src');
    var captchaInput = document.getElementsByClassName('captcha-input')[0];
    var captchaRefreshIcon = document.getElementsByClassName('refresh-icon')[0];
    var refreshCaptcha = function () {
        captchaInput.value = '';
        var newCaptchaImageSrc = captchaImageSrc + "?t=" + Date.now();
        captchaImage.setAttribute('src', newCaptchaImageSrc);
    };
    captchaImage.addEventListener('load', function () {
        captchaInput.focus();
    });
    captchaRefreshIcon.addEventListener('click', refreshCaptcha);
});
//# sourceMappingURL=captcha.js.map