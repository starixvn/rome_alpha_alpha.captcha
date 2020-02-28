$(() => {
    const captchaImagePlacehoder = $('.captcha-image');
    const captchaRefreshIcon = $('.captcha-input a');
    const captchaInput = $('.captcha-input input');

    const registerImageEvent = () => {
        const captchaImage = captchaImagePlacehoder.find('img');
        captchaImage.load(() => {
            captchaInput.focus();
        });
    }

    const freshCaptcha = () => {
        captchaInput.val('');
        const captchaImageHtml = captchaImagePlacehoder.html();
        captchaImagePlacehoder.html('');
        captchaImagePlacehoder.append(captchaImageHtml);
        registerImageEvent();
    }

    captchaRefreshIcon.click(freshCaptcha);
});