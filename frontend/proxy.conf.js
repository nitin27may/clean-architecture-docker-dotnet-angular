const PROXY_CONFIG = [
  {
    context: ['/api'],
    target: process.env['services__contact-api__https__0'] || 
            process.env['services__contact-api__http__0'] || 
            'http://localhost:5217',
    secure: false,
    changeOrigin: true,
    logLevel: 'debug',
    ws: true
  }
];

console.log('API Proxy Target:', PROXY_CONFIG[0].target);

module.exports = PROXY_CONFIG;
