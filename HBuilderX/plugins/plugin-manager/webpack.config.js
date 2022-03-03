const path = require('path');

module.exports = {
  entry: './main.js',
  target:'node',
  output: {
    path: __dirname,
    filename: 'out.js'
  },
  node: {
    __filename: false,
    __dirname: false
  }
};