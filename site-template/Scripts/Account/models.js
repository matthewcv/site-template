

(function() {
    window.account = {};

    window.account.profile = function () {
        var that = this;

        this.EmailAddress = ko.observable()
            .extend({
                validate: {
                    email: {
                        message: "Gotta be a real email address",
                        when: function () {
                            return $.trim(that.EmailAddress()).length > 0;
                        }
                    }
                }
            });
        this.DisplayName = ko.observable()
            .extend({ validate: { required: "I need to know your name." } });
        this.Location = ko.observable();
    };


    window.account.oauthIdentity = function() {

    };    

    
})();