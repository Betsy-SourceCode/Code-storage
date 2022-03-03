const { getLanguageService, JSONSchema, TextDocument, MatchingSchema } = require("vscode-json-languageservice");

const schema = require('./node.package.schema')
const fs = require('fs')

/**
 * @param {string} jsonContentUri
 * @param {string} jsonContent
 * @param {string} jsonSchema
 * @param {stirng[]} fileMatch
 */ 
async function validate(jsonContentUri, jsonContent, jsonSchema, fileMatch) {
  const textDocument = TextDocument.create(jsonContentUri, 'json', 1, jsonContent);
  jsonSchema = jsonSchema || schema
  fileMatch = fileMatch || ['package.json']
  const jsonLanguageService = getLanguageService({
      schemaRequestService: (uri) => {
          if (uri === '') {
              return Promise.resolve(JSON.stringify(jsonSchema));
          }
          return Promise.reject(`Unabled to load schema at ${uri}`);
      }
  });

  jsonLanguageService.configure({ allowComments: true, schemas: [{ fileMatch: fileMatch, uri: '' }] });
  const jsonDocument = jsonLanguageService.parseJSONDocument(textDocument);

  const diagnostics = await jsonLanguageService.doValidation(textDocument, jsonDocument);
  // console.log('Validation results:', diagnostics.map(d => `[line ${d.range.start.line}] ${d.message}`));

  /*
   * > Validation results: [
   * >    '[line 1] Incorrect type. Expected "string".',
   * >    '[line 2] Expected comma'
   * > ]
   */
  return diagnostics.map(d => {
    return {line: d.range.start.line, message: d.message}
  })
}

/**
 * @param {string} jsonContentUri
 * @param {string} jsonContent
 * @param {number} line
 * @param {number} character
 * @param {string} jsonSchema
 * @param {stirng[]} fileMatch
 */ 
async function completion(jsonContentUri, jsonContent, line, character, jsonSchema, fileMatch) {
  const textDocument = TextDocument.create(jsonContentUri, 'json', 1, jsonContent);
  jsonSchema = jsonSchema || schema;
  fileMatch = fileMatch || ['package.json'];
  const jsonLanguageService = getLanguageService({
      schemaRequestService: (uri) => {
          if (uri === '') {
              return Promise.resolve(JSON.stringify(jsonSchema));
          }
          return Promise.reject(`Unabled to load schema at ${uri}`);
      }
  });

  jsonLanguageService.configure({ allowComments: true, schemas: [{ fileMatch: fileMatch, uri: '' }] });
  const jsonDocument = jsonLanguageService.parseJSONDocument(textDocument);
  const completionResult = await jsonLanguageService.doComplete(textDocument, { line: line, character: character }, jsonDocument);

  return completionResult ? completionResult.items.map(i => `${i.label}`) : {}
  /*
   * Completion proposals: [ '"Ireland"', '"Iceland"' ]
   */
}

module.exports = {
  doValidate: validate,
  doCompletion: completion
}