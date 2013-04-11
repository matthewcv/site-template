(function() {
    var cc = window.candyCount = {};

    cc.candy = function() {
        this.Id = 0;
        this.Name = ko.observable();
        this.Colors = ko.observableArray(); //array of cc.candyColor
        this.Total = ko.observable();
    };

    cc.candyColor = function(name) {
        this.Name = ko.observable(name);
        this.Total = ko.observable();
    };

    cc.countedCandy = function () {
        var that = this;
        this.Id = 0;
        this.Candy = ko.observable();
        this.Candy.subscribe(function() {
            that.Colors.removeAll();
            console.log('change');
            $.each(that.Candy().Colors(), function(i, v) {
                that.Colors.push(new cc.candyColor(v.Name()));
            });
        });
        this.Colors = ko.observableArray();  //array of cc.candyColor

    };



})();