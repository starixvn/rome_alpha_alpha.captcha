document.addEventListener("DOMContentLoaded", () => {
    let submitted = false;
    const captchaContainer = document.getElementsByClassName('captcha-container')[0];
    const captchaImage = captchaContainer.getElementsByClassName('captcha-image')[0];
    const captchaImageSrc = captchaImage.getAttribute('src');
    const captchaInput = captchaContainer.getElementsByClassName('captcha-input')[0] as HTMLInputElement;
    const captchaRefreshIcon = captchaContainer.getElementsByClassName('captcha-icon-refresh')[0];

    captchaImage.addEventListener('load', () => {
        captchaInput.focus();
    });

    captchaRefreshIcon.addEventListener('click', () => {
        if (submitted) {
            return;
        }

        captchaInput.value = '';
        captchaInput.classList.remove('invalid');
        const newCaptchaImageSrc = `${captchaImageSrc}${captchaImageSrc.indexOf('?') === -1 ? '?' : '&'}t=${Date.now()}`;
        captchaImage.setAttribute('src', newCaptchaImageSrc);
    });

    if (captchaContainer.tagName.toUpperCase() === 'FORM') {
        const captchaForm = captchaContainer as HTMLFormElement;
        captchaForm.addEventListener('submit', e => {
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
            const elements = captchaForm.elements;
            for (let i = 0, len = elements.length; i < len; ++i) {
                const element = elements[i] as any;
                if (element.type === 'submit') {
                    element.disabled = true;
                }
            }
        });
    }
});