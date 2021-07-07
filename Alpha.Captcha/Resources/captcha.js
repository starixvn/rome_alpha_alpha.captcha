document.addEventListener("DOMContentLoaded", function () {
    var submitted = false;
    var captchaContainer = document.getElementsByClassName('captcha-container')[0];
    var captchaImage = captchaContainer.getElementsByClassName('captcha-image')[0];
    var captchaImageSrc = captchaImage.getAttribute('src');
    var captchaInput = captchaContainer.getElementsByClassName('captcha-input')[0];
    var captchaRefreshIcon = captchaContainer.getElementsByClassName('captcha-icon-refresh')[0];
    captchaImage.addEventListener('load', function () {
        captchaInput.focus();
    });
    captchaRefreshIcon.addEventListener('click', function () {
        if (submitted) {
            return;
        }
        captchaInput.value = '';
        captchaInput.classList.remove('invalid');
        var newCaptchaImageSrc = "" + captchaImageSrc + (captchaImageSrc.indexOf('?') === -1 ? '?' : '&') + "t=" + Date.now();
        captchaImage.setAttribute('src', newCaptchaImageSrc);
    });
    if (captchaContainer.tagName.toUpperCase() === 'FORM') {
        var captchaForm_1 = captchaContainer;
        var updateSubmitStatus_1 = function (disabled) {
            var elements = captchaForm_1.elements;
            for (var i = 0, len = elements.length; i < len; ++i) {
                var element = elements[i];
                if (element.type === 'submit') {
                    element.disabled = disabled;
                }
            }
        };
        updateSubmitStatus_1(false);
        captchaForm_1.addEventListener('submit', function (e) {
            if (submitted) {
                e.preventDefault();
                return;
            }
            if (!captchaInput.value) {
                e.preventDefault();
                if (!captchaInput.classList.contains('invalid')) {
                    captchaInput.classList.add('invalid');
                }
                return;
            }
            submitted = true;
            captchaInput.classList.remove('invalid');
            updateSubmitStatus_1(true);
        });
    }
});
