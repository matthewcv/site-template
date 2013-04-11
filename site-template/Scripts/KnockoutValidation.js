(function() {

    ko.extenders.validate = function(target, option) {

        target['valConfig'] = option;
        ensureProperty(target);
        return target;
    };

    
    function getValSettings(obj) {
        /*obj is going to be a KO observable or an object assumed to be in the form
            {
               path: someObservable,
               css: 'someClass'  //this is optional
            }
        */
        var settings = obj;
        if (obj.toString().indexOf("observable") >= 0) {
            settings = {
                path: obj,
                css: 'invalid'
            };
        }
        if (settings.path === undefined) {
            throw "you didn't define a path for validation";
        }
        return settings;
    }
    
    ko.bindingHandlers.validate = {
        init: function(element, valAccess, allBindingAccess, viewModel, bindingContext) {
            ensureViewModel(viewModel);
            ensureProperty(getValSettings(valAccess()).path);
        },
        update: function(element, valAccess, allBindingAccess, viewModel, bindingContext) {
            
            var settings = getValSettings(valAccess());
            var $e = $(element).removeClass(settings.css);
            var valid = validate(settings, viewModel);
            
            if (!valid) {
                $e.addClass(settings.css);
            }
        }
    };


    function validate(valSettings, viewModel) {
        var property = valSettings.path;
        var valConfig = property.valConfig;
        
        if (property.validationMessages().length > 0) {
            property.validationMessages.removeAll();
            var toRemove = [];
            for (var i = 0; i < viewModel.validationMessages().length; i++) {
                if (viewModel.validationMessages()[i].property == property) {
                    toRemove.push(viewModel.validationMessages()[i]);
                }
            }
            viewModel.validationMessages.removeAll(toRemove);
        }
        required(property, viewModel, normalizedIndividualConfig(valConfig.required));
        email(property, viewModel, normalizedIndividualConfig(valConfig.email));

        return property.isValid();
    }

    function normalizedIndividualConfig(config) {
        if (config === undefined) {
            return config;
        }
        var s = config;
        if (typeof (config) == "string") {
            s = {
                message: config
            };
        }
        
        if (s.when === undefined) {
            s.when = function() {
                return true;
            };
        }
        return s;
    }

    var emailRegex = new RegExp("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
    function email(property, viewModel, config) {
        if (config && config.when()) {
            if (!emailRegex.test(property())) {
                addMessages(property, viewModel, config, "email");
            }
        }
    }

    function required(property, viewModel, config) {
        if (config && config.when()) {
            var val = property();
            val = $.trim(val);
            if (val == null || val == "") {
                addMessages(property, viewModel, config, "required");
            }
        }
    }
    
    
    
    function addMessages(property, viewModel, config, valType) {
        property.validationMessages.push(config.message);
        viewModel.validationMessages.push({ property: property, message: config.message, valType: valType });
    }

    function ensureViewModel(viewModel) {
        if (viewModel.validationMessages === undefined) {
            viewModel.validationMessages = ko.observableArray();
            viewModel.isValid = ko.observable(true);

            var vm = viewModel;
            viewModel.validationMessages.subscribe(function(v) {
                vm.isValid(v.length === 0);
            });
        }
    }
    function ensureProperty(property)
    {
        if (property.validationMessages === undefined) {
            property.validationMessages = ko.observableArray();
            property.isValid = ko.observable(true);
            property.isInValid = ko.observable(false);
            var p = property;
            property.validationMessages.subscribe(function (v) {
                p.isValid(v.length === 0);
                p.isInValid(v.length > 0);
            });
        }

    }
    

})();