$(() => {
    const captchaImageRow = $('.row-image');
    const captchaRefreshIcon = $('.refresh-icon');
    const captchaInput = $('.captcha-input');

    const registerImageEvent = () => {
        const captchaImage = $('.captcha-image');
        captchaImage.load(() => {
            captchaInput.focus();
        });
    }

    const freshCaptcha = () => {
        captchaInput.val('');
        const newCaptchaImage = $('<div>').append($('.captcha-image').clone()).html();
        $('.captcha-image').remove();
        captchaImageRow.prepend(newCaptchaImage);
        registerImageEvent();
    }

    captchaRefreshIcon.click(freshCaptcha);
});