(function (window) {
    window.__env = window.__env || {};
    window.__env.apiUrl = window.location.href;

    if (window.location.href.indexOf('localhost:5002') !== -1) {
        // Dev environment
        window.__env.enableDebug = true;
        window.__env.envName = 'dev';
    } else if (window.location.href.indexOf('-uat') !== -1) {
        // UAT environment
        window.__env.enableDebug = false;
        window.__env.envName = 'uat';
    } else {
        // Production environment
        window.__env.enableDebug = false;
        window.__env.envName = 'prd';
    }
}(this));