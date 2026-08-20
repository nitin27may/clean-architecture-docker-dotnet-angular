// Aspire 13's AddJavaScriptApp (Aspire.Hosting.JavaScript) injects service-discovery
// env vars as CONTACT_API_HTTPS / CONTACT_API_HTTP (upper-snake-case) — a different,
// shell-safe convention from the old AddNpmApp/Aspire.Hosting.NodeJs one, which used
// dotted names like services__contact-api__https__0 (not usable in Aspire 13, and not
// a valid shell identifier to begin with, since resource names may contain hyphens).
const PROXY_CONFIG = [
  {
    context: ['/api'],
    target: process.env['CONTACT_API_HTTPS'] ||
            process.env['CONTACT_API_HTTP'] ||
            'http://localhost:5217',
    secure: false,
    changeOrigin: true,
    logLevel: 'debug',
    ws: true
  }
];

console.log('API Proxy Target:', PROXY_CONFIG[0].target);

module.exports = PROXY_CONFIG;
