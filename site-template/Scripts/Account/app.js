(function() {

    

    account.app = new app();

    toastr.options.target = "#toastr-target";

    function app() {
        this.start = function (profile) {
            this.viewModel = {
                profile: profile,
                

                saveDisabled:ko.observable(false),
                
                saveText: ko.observable("Save"),
                
                saveProfile: function() {
                        console.log("valid " + this.profile.DisplayName.isValid());
                    this.saveDisabled(true);
                    this.saveText("Saving Profile...");
                    var that = this;

                    $.ajax("/Account/Edit", {
                        contentType: "application/json",
                        data: ko.toJSON(that.profile),
                        type:"POST"
                    })
                    .done(function(d,s,x) {
                        that.profileSaved(d, s, x);
                    })
                    .always(function(d,s,x) {
                        that.saveDisabled(false);
                        that.saveText("Save");
                    })
                        ;

                },
                
                profileSaved:function(data, status, xhr)
                {
                    if (data.SaveSuccess) {
                        toastr.success("Profile saved.");
                    } else {
                        toastr.error(data.Errors, "Error saving profile");
                    }
                }
            };


            ko.applyBindings(this.viewModel, $("#accountEdit")[0]);

        };
    }

})();