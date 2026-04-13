function getTurnstileResponse() {
    const input = document.querySelector('[name="cf-turnstile-response"]');
    return input ? input.value : '';
}

function resetTurnstile() {
    if (typeof turnstile !== 'undefined') {
        turnstile.reset();
    }
}
