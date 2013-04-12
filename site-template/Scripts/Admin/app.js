(function() {

    

    admin.app = new app();

    toastr.options.target = "#toastr-target";

    function viewModel() {
        var that = this;
    }

    function app() {
        var that = this;
        this.viewModel = new viewModel();
        this.sammy = getSammy();
        this.start = function () {


        };

        function getSammy() {
            return Sammy()
                .route("GET", "/admin", _index)
                .route("GET", "/admin/Index", _index)
                ;
        }
        
        function _index(ctx) {

        }

    }

    $(function() {
        admin.app.start();
    });

})();