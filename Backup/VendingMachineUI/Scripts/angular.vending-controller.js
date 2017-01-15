var app = angular.module('coffeeApp', []);

app.factory('vendingService', ['$http', function($http) {
	var service = {
		getCoffee: function() {
			return $http.get('/Coffee/GetCoffee');
		},
		
		getCream: function() {
			return $http.get('/Cream/GetCream');
		},
		
		getSugar: function() {
			return $http.get('/Sugar/GetSugar');
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
		orders: []
	};
    
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
    
    ctrl.orderTotal = function() {
    	return 10;
    }
}]);