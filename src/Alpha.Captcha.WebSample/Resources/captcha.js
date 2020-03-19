document.addEventListener("DOMContentLoaded", function () {
    var captchaContainer = document.getElementsByClassName('captcha-container')[0];
    var captchaImage = captchaContainer.getElementsByClassName('captcha-image')[0];
    var captchaImageSrc = captchaImage.getAttribute('src');
    var captchaInput = captchaContainer.getElementsByClassName('captcha-input')[0];
    var captchaRefreshIcon = captchaContainer.getElementsByClassName('captcha-icon-refresh')[0];
    captchaImage.addEventListener('load', function () {
        captchaInput.focus();
    });
    captchaRefreshIcon.addEventListener('click', function () {
        captchaInput.value = '';
        captchaInput.classList.remove('invalid');
        var newCaptchaImageSrc = '';
        if (captchaImageSrc.indexOf('?') === -1) {
            newCaptchaImageSrc = captchaImageSrc + "?t=" + Date.now();
        }
        else {
            newCaptchaImageSrc = captchaImageSrc + "&t=" + Date.now();
        }
        captchaImage.setAttribute('src', newCaptchaImageSrc);
    });
    if (captchaContainer.tagName.toUpperCase() === 'FORM') {
        var captchaForm = captchaContainer;
        captchaForm.addEventListener('submit', function (e) {
            if (!captchaInput.value) {
                e.preventDefault();
                if (!captchaInput.classList.contains('invalid')) {
                    captchaInput.classList.add('invalid');
                }
            }
            else {
                captchaInput.classList.remove('invalid');
            }
        });
    }
});