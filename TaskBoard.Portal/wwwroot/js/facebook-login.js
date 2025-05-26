function initializeFacebookLogin(dotNetHelper) {
    console.log("Facebook initialize başlatıldı");

    // Eger SDK zaten yuklu degilse yukler...
    if (!document.getElementById('facebook-jssdk')) {
        let js = document.createElement('script');
        js.id = 'facebook-jssdk';
        js.src = 'https://connect.facebook.net/en_US/sdk.js';
        document.body.appendChild(js);
    }

    // SDK hazir mi kontrolu yapiliyor..
    let checkFBReady = setInterval(() => {
        if (window.FB) {
            clearInterval(checkFBReady); // FB hazirsa interval durdurulur

            console.log("FB bulundu, init ediliyor...");

            FB.init({
                appId: '1479151056389713',
                cookie: true, // cerez kullanimini etkinleştirir
                xfbml: false, // custom buton kullandigim icin false
                version: 'v22.0' // Facebook Graph API versiyonu
            });

            // Buton render ediliyor
            const fbButtonDiv = document.getElementById('facebookLoginDiv');
            if (fbButtonDiv) {
                fbButtonDiv.innerHTML = `
                    <button id="fbCustomLoginBtn" style="
                        padding: 10px 20px;
                        background-color: #1877f2;
                        color: white;
                        border: none;
                        border-radius: 4px;
                        cursor: pointer;
                    ">Facebook ile Giriş Yap</button>`;

                // Butona tiklanirsa FB.login metodu cagrilir
                document.getElementById('fbCustomLoginBtn').addEventListener('click', function () {
                    FB.login(function (response) {
                        if (response.authResponse) {
                            console.log("FB login başarılı");
                            dotNetHelper.invokeMethodAsync("OnFacebookLoginSuccess", response.authResponse.accessToken);
                        } else {
                            console.error('Facebook login failed');
                        }
                    }, { scope: 'email' });
                });
            }
        }
    }, 500); // SDK yuklenene kadar her 500ms'de kontrol eder
}

