(function() {
    candyCount.app = new Application();
    var cc = candyCount;
    function ViewModel(app) {
        this.app = app;
        var that = this;
        this.countedCandy = ko.observable();//an instance of cc.countedCandy
        this.candyType = ko.observable();//an instance of cc.candy used for adding a new candy to the system.  m&m's, skittles, etc.
        this.candyTypes = ko.observableArray();//an array of cc.candy used for selecting which candy you want to add a count for.
        
        this.makeNewCandyType = function() {
            var c = new cc.candy();
            c.Name("M&Ms");
            c.ColorNames = ko.observable("Red,Brown,Green,Blue,Orange,Yellow");
            that.candyType(c);
        };

        this.madeNewCandyType = function() {

            var colors = that.candyType().ColorNames().split(',');

            for (var i = 0; i < colors.length; i++) {
                var color = $.trim(colors[i]);
                if (color.length > 0) {
                    var c = new cc.candyColor();
                    c.Name(color);
                    that.candyType().Colors.push(c);
                }
            }

            that.candyTypes.push(that.candyType());
            that.candyType(null);


        };
    }
    
    function Application() {
        var that = this;
        this.viewModel = new ViewModel(this);
        this.sammy = _sammy();

        this.start = function(appRoot) {
            that.sammy.element_selector = appRoot;
            that.sammy.run();
            ko.applyBindings(this.viewModel, $(appRoot)[0]);
        };
        

        function _sammy() {
            return Sammy()
            .route("GET", "/CandyCount", _index)
            .route("GET", "/CandyCount/Index", _index)

            .route("GET", "/CandyCount/Add", _add);
       
        }

        function _add(ctx) {
            var vm = that.viewModel;

            vm.countedCandy(new cc.countedCandy());
        }

        function _index(ctx) {
            var vm = that.viewModel;
            vm.countedCandy(null);
        }


    }
    
})();