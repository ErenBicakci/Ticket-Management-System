const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    proxy: {
      '/api': {
        target: 'http://localhost:9080',
        changeOrigin: true,
        secure: false,
        logLevel: 'debug',
        onProxyReq: (proxyReq, req, res) => {
          console.log('Proxying request to:', proxyReq.getHeader('host') + proxyReq.path);
        },
        onProxyRes: (proxyRes, req, res) => {
          console.log('Proxy response status:', proxyRes.statusCode);
        }
      }
    }
  }
})
