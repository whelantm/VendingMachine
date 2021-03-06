﻿var app = angular.module('coffeeApp', []);

app.factory('vendingService', ['$http', function($http) {
	var service = {
		getCoffee: function() {
			return $http.get('/RhinoCoffee/Coffee/GetCoffee');
		},
		
		getCream: function() {
			return $http.get('/RhinoCoffee/Cream/GetCream');
		},
		
		getSugar: function() {
			return $http.get('/RhinoCoffee/Sugar/GetSugar');
		}
		
	}
	
	return service;
}]);

app.controller('vendingController', ['vendingService', function(vendingService) {
    var ctrl = this;
    
    ctrl.model = {
		currentOrder: {
			id: 0,
			coffee: { Quantity: 0, Price: 0 },
			cream: { Quantity: 0, Price: 0 },
			sugar: { Quantity: 0, Price: 0 }
		},
		orders: [],
        payment: {
            twenties: 0,
            tens: 0,
            fives: 0,
            ones: 0,
            quarters: 0,
            dimes: 0,
            nickels: 0
        }
	};

    ctrl.isPaying = false;

    ctrl.addCoffee = function() {
    	vendingService.getCoffee().then(function(result) {
    		ctrl.model.currentOrder.coffee.Price = result.data.Price;
    		ctrl.model.currentOrder.coffee.Quantity = 1;
    	});
    }
    
    ctrl.addCream = function() {
    	if (ctrl.model.currentOrder.coffee.Quantity == 0) {
    		alert('Add coffee first');
    		return;
    	}
    	
    	vendingService.getCream().then(function(result) {
    		ctrl.model.currentOrder.cream.Price += result.data.Price;
    		ctrl.model.currentOrder.cream.Quantity++;
    	});
    }
    
    ctrl.addSugar = function() {
    	if (ctrl.model.currentOrder.coffee.Quantity == 0) {
    		alert('Add coffee first');
    		return;	
    	}
    	
    	vendingService.getSugar().then(function(result) {
    		ctrl.model.currentOrder.sugar.Price += result.data.Price;
    		ctrl.model.currentOrder.sugar.Quantity++;
    	});
    }
    
    ctrl.addToOrder = function() {
    	ctrl.model.orders.push(angular.copy(ctrl.model.currentOrder));
    	ctrl.model.currentOrder.coffee.Price = 0;
    	ctrl.model.currentOrder.coffee.Quantity = 0;
    	
    	ctrl.model.currentOrder.cream.Price = 0;
    	ctrl.model.currentOrder.cream.Quantity = 0;
    	
    	ctrl.model.currentOrder.sugar.Price = 0;
    	ctrl.model.currentOrder.sugar.Quantity = 0;
    	ctrl.model.currentOrder.id++;
    }
    
    ctrl.clearCurrentOrder = function() {
        ctrl.model.currentOrder.coffee.Price = 0;
    	ctrl.model.currentOrder.coffee.Quantity = 0;
    	
    	ctrl.model.currentOrder.cream.Price = 0;
    	ctrl.model.currentOrder.cream.Quantity = 0;
    	
    	ctrl.model.currentOrder.sugar.Price = 0;
    	ctrl.model.currentOrder.sugar.Quantity = 0;
    }
    
    ctrl.orderTotal = function () {
        var total = 0;
    	for (var i = 0; i < ctrl.model.orders.length; i++) {
	        total = total + ctrl.model.orders[i].coffee.Price;
	        total = total + ctrl.model.orders[i].cream.Price;
	        total = total + ctrl.model.orders[i].sugar.Price;
    	}
        return total;
    }

    ctrl.cancelOrder = function() {
        ctrl.model = {
            currentOrder: {
                id: 0,
                coffee: { Quantity: 0, Price: 0 },
                cream: { Quantity: 0, Price: 0 },
                sugar: { Quantity: 0, Price: 0 }
            },
            orders: []
        };

        ctrl.isPaying = false;
    }

    ctrl.completeOrder = function() {
        ctrl.isPaying = true;
    }

    ctrl.paymentTotal = function() {
        var total = 0;
        total = (total + ctrl.model.payment.fives) * 5;

        total = total + (ctrl.model.payment.twenties * 20);
        total = total + (ctrl.model.payment.tens * 10);
        
        total = total + ctrl.model.payment.ones;

        total = total + (ctrl.model.payment.quarters * .25);
        total = total + (ctrl.model.payment.dimes * .1);
        total = total + (ctrl.model.payment.nickels * .05);

        return total;
    }

    ctrl.addTwenty = function() {
        ctrl.model.payment.twenties++;
    }

    ctrl.addTen = function() {
        ctrl.model.payment.tens++;
    }

    ctrl.addFive = function() {
        ctrl.model.payment.fives++;
    }

    ctrl.addOne = function() {
        ctrl.model.payment.ones++;
    }

    ctrl.addQuarter = function() {
        ctrl.model.payment.quarters++;
    }

    ctrl.addDime = function() {
        ctrl.model.payment.dimes++;
    }

    ctrl.addNickel = function() {
        ctrl.model.payment.nickels++;
    }
}]);