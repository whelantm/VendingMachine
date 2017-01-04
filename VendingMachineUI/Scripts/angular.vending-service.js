var app = angular.module('coffeeApp', []);

app.controller('vendingController', function() {
    var ctrl = this;
    
    ctrl.model = {
		currentOrder: {
			coffee: 0,
			cream: 0,
			sugar: 0
		},
		order: []
	};
    
    ctrl.addCoffee = function() {
    	ctrl.model.currentOrder.coffee = 1;
    }
    
    ctrl.addCream = function() {
    	if (ctrl.model.currentOrder.coffee == 0) {
    		alert('Add coffee first');
    		return;
    	}
    	ctrl.model.currentOrder.cream++;
    }
    
    ctrl.addSugar = function() {
    	if (ctrl.model.currentOrder.coffee == 0) {
    		alert('Add coffee first');
    		return;	
    	}
    	ctrl.model.currentOrder.sugar++;
    }
    
    ctrl.addToOrder = function() {
    	ctrl.model.order.push(ctrl.model.currentOrder);
    	ctrl.model.currentOrder.coffee = 0;
    	ctrl.model.currentOrder.cream = 0;
    	ctrl.model.currentOrder.sugar = 0;
    }
    
    ctrl.clearCurrentOrder = function() {
        ctrl.model.currentOrder.coffee = 0;
    	ctrl.model.currentOrder.cream = 0;
    	ctrl.model.currentOrder.sugar = 0;
    }
});