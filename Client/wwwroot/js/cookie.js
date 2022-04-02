window.methods = {
    CreateCookie: function (name, value, days) {
        var expires;
        if (days) {
            const date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = `; expires=${date.toGMTString()}`;
        } else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    },
    GetCookie: function (cname) {
        const name = cname + "=";
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(";");
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === " ") {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    DeleteCookie: function (cname) {
        const name = cname + "=";
        const expires = "; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
        document.cookie = name + expires;
    },
    LogNav: function (name) {
        window.gtag("set", "page_path", name);
        window.gtag("event", "page_view");
    }
}
window.methods = {
    CreateCookie: function (name, value, days) {
        var expires;
        if (days) {
            const date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = `; expires=${date.toGMTString()}`;
        } else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    },
    GetCookie: function (cname) {
        const name = cname + "=";
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(";");
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === " ") {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    DeleteCookie: function (cname) {
        const name = cname + "=";
        const expires = "; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
        document.cookie = name + expires;
    },
    LogNav: function (name) {
        window.gtag("set", "page_path", name);
        window.gtag("event", "page_view");
    }
}