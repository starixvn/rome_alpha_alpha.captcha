document.addEventListener("DOMContentLoaded", () => {
    const captchaImage = document.getElementsByClassName('captcha-image')[0];
    const captchaImageSrc = captchaImage.getAttribute('src');
    const captchaInput = document.getElementsByClassName('captcha-input')[0] as HTMLInputElement;
    const captchaRefreshIcon = document.getElementsByClassName('refresh-icon')[0];

    const refreshCaptcha = () => {
        captchaInput.value = '';
        const newCaptchaImageSrc = `${captchaImageSrc}?t=${Date.now()}`;
        captchaImage.setAttribute('src', newCaptchaImageSrc);
    }

    captchaImage.addEventListener('load', () => {
        captchaInput.focus();
    });

    captchaRefreshIcon.addEventListener('click', refreshCaptcha);
});