// Toast Auto-Dismiss
document.addEventListener("DOMContentLoaded", function () {
    const toasts = document.querySelectorAll('.vt-toast');
    toasts.forEach(toast => {
        setTimeout(() => {
            toast.classList.add('hide');
            toast.addEventListener('animationend', () => {
                toast.remove();
            });
        }, 5000); // 5 segundos para errores/success
    });
});
