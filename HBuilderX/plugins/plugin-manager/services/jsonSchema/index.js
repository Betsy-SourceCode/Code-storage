const { doValidate, doCompletion } = require('./server.js')

function _init(conn){
	conn.onRequest('json/validate', async function(params){
    return await doValidate(params.jsonContentUri, params.jsonContent, JSON.parse(params.jsonSchema), params.fileMatch)
  });

  conn.onRequest('json/completion', async function(params) {
    // writeFileSync(path.join(__dirname + "/log.txt"), JSON.stringify(params, null, 2));
    // console.log(params)
    return await doCompletion(params.jsonContentUri, params.jsonContent, params.line, params.character, params.jsonSchema ? JSON.parse(params.jsonSchema) : undefined, params.fileMatch)
  })
}
module.exports = {
	init: _init
};