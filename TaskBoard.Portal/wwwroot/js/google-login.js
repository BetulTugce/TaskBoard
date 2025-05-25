function initializeGoogleLogin(dotNetHelper) {
    console.log("initializeGoogleLogin çağrıldı");

    if (!window.google || !google.accounts || !google.accounts.id) {
        console.error("Google Identity Services script yüklenmedi veya hazır değil.");
        return;
    }

    google.accounts.id.initialize({
        client_id: "372246186092-vrqp6i0ua2vpiu8ubcqk6ajumflkmpdq.apps.googleusercontent.com",
        callback: function (response) {
            dotNetHelper.invokeMethodAsync('OnGoogleLoginSuccess', response);
        }
    });

    google.accounts.id.renderButton(
        document.getElementById("googleLoginDiv"),
        { theme: "outline", size: "large" }
    );

    console.log("Google button render edildi");

}

