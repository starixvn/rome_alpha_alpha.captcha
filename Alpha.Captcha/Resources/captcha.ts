document.addEventListener("DOMContentLoaded", () => {
    const captchaContainer = document.getElementsByClassName('captcha-container')[0];
    const captchaImage = captchaContainer.getElementsByClassName('captcha-image')[0];
    const captchaImageSrc = captchaImage.getAttribute('src');
    const captchaInput = captchaContainer.getElementsByClassName('captcha-input')[0] as HTMLInputElement;
    const captchaRefreshIcon = captchaContainer.getElementsByClassName('captcha-icon-refresh')[0];

    captchaImage.addEventListener('load', () => {
        captchaInput.focus();
    });

    captchaRefreshIcon.addEventListener('click', () => {
        captchaInput.value = '';
        captchaInput.classList.remove('invalid');
        const newCaptchaImageSrc = `${captchaImageSrc}${captchaImageSrc.indexOf('?') === -1 ? '?' : '&'}t=${Date.now()}`;
        captchaImage.setAttribute('src', newCaptchaImageSrc);
    });

    if (captchaContainer.tagName.toUpperCase() === 'FORM') {
        const captchaForm = captchaContainer as HTMLFormElement;
        captchaForm.addEventListener('submit', e => {
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