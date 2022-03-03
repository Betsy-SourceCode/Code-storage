const hbx = require('./hbxBridge.js');
const qDebug = hbx.qDebug;
const constants = require('./constants.js');
const metatypes = require("./api/metatypes.js");
let plugins = {
};
//webpack打包时部分require不能被替换。
const requireFunc = (typeof __webpack_require__ === 'function') ? __non_webpack_require__ : require;
function PluginContext(id){
    this.subscriptions = [];
    this.id = id;
    this.dispose = function(){
        if(this.subscriptions.length > 0){
            let pluginId = this.id;
            this.subscriptions.forEach(function(item){
                if(item && item.dispose && typeof item.dispose === 'function'){
                    item.dispose();
                }
            });
        }
    };
}

function _init(connection){
	connection.onRequest("plugin/activate", function(param){
        try{
            return _activatePlugin(param);
        }catch(e){
            console.error(e);
        }
        return false;
	});
	//deactivate
	connection.onRequest("plugin/deactivate", function(param){
	    console.log("deactivate plugin:" + JSON.stringify(param));
	    return _deactivatePlugin(param);
	});
    
    connection.onRequest("plugin/invokeApi",function(param){
        let pluginId = param.pluginId;
        let fnName = param.fn;
        if(plugins[pluginId] 
            && plugins[pluginId].exports
            && plugins[pluginId].exports.hasOwnProperty(fnName)
            && (typeof plugins[pluginId].exports[fnName] == "function")){
            let apiFn = plugins[pluginId].exports[fnName];
            return apiFn(metatypes.newObject(param.args,true));
        }
        console.error("Plugin [" + pluginId + "] has no exports api:" + fnName + ".");
    });
}

function _activatePlugin(pluginInfo){
    const id = pluginInfo.id;
    if(!id || id.length == 0){
        console.error("Must contains a valid id in plugin info!");
        return false;
    }
	let pluginsDir = constants.pluginsDir;
	let pluginPath = pluginsDir + '/' + id;
	if(pluginInfo.path){
		pluginPath = pluginInfo.path;
	}
	const plugin = requireFunc(pluginPath);
	if(!plugin || !plugin.activate){
		console.error("Not a valid plugin package.");
		return false;
	}
    if(plugin.activated){
        console.error(`Plugin [${id}] has already activated,won't activate again.`);
        return true;
    }
	plugins[id] = {
		instance:plugin,
		context:new PluginContext(id)
	};
	if(plugin && plugin.activate && !plugin.activated ){
		let api = plugin.activate(plugins[id].context);
        if(api){
            plugins[id].exports = api;
        }
		plugin.activated = true;
	}
    return plugin.activated;
}

function _deactivatePlugin(pluginInfo){
    const id = pluginInfo.id;
    if(!id || id.length == 0){
        console.error("Must contains a valid id in plugin info!");
        return false;
    }
    try{
        const pluginBundle = plugins[id];
        if(!pluginBundle || !pluginBundle.instance){
            console.error("Not a valid plugin package.");
            return false;
        }
        let plugin = pluginBundle.instance;
        if(plugin.deactivate && plugin.activated){
            plugin.deactivate();
        }
        plugin.activated = false;
        pluginBundle.context.dispose();
        plugins[id] = undefined;
        //要删除require的缓存，插件单独升级或者卸载重装时需要重新加载。
        let pluginsDir = constants.pluginsDir;
        let pluginPath = pluginsDir + '/' + id;
        delete requireFunc.cache[requireFunc.resolve(pluginPath)];
    }catch(e){
        //TODO handle the exception
        console.error(e.message);
        return false;
    }
    return true;
}

function _getExtension(plugnId) {
    let api = plugins[plugnId];
    if(api)
    {
        return new Proxy(api.exports, {
            get: function(target, key) {
                if (key in target) {
                    return target[key];
                }
            }
        });
    }
    return undefined;
}

function existsPlugin(pluginId) {
   let result = new Promise((resolve, reject) => {
   	hbx.request("pluginmangerhost.existsPlugin",
   		{pluginId:pluginId}).then((data) => {
   		resolve(data);
   	}, reject);
   });
   return result
}

function installPlugin(pluginId) {
   let result = new Promise((resolve, reject) => {
   	hbx.request("pluginmangerhost.installPlugin",
   		{pluginId:pluginId}).then((data) => {
   		resolve(data);
   	}, reject);
   });
   return result
}

module.exports = {
	init:_init,
    activatePlugin:_activatePlugin,
    deactivatePlugin:_deactivatePlugin,
    getExtension: _getExtension,
    existsPlugin:existsPlugin,
    installPlugin:installPlugin
};