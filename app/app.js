/// <reference path="../Scripts/angular-1.1.4.js" />

/*#######################################################################
  
  Dan Wahlin
  http://twitter.com/DanWahlin
  http://weblogs.asp.net/dwahlin
  http://pluralsight.com/training/Authors/Details/dan-wahlin

  Normally like to break AngularJS apps into the following folder structure
  at a minimum:

  /app
      /controllers      
      /directives
      /services
      /partials
      /views

  #######################################################################*/

var app = angular.module('requestsApp', ['ngRoute']);

//This configures the routes and associates each route with a view and a controller
app.config(function ($routeProvider) {
    $routeProvider
    
        //for standard user
        .when('/standard',
            {
                controller: 'MainController',
                templateUrl: '/app/partials/standard.html'
            })
        //view for standard user seeing request
        .when('/standardview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: '/app/partials/standardview.html'
            })
            
        //for manager user
        .when('/manager',
            {
                controller: 'MainController',
                templateUrl: '/app/partials/manager.html'
            })
        //view for manager user seeing request
        .when('/managerview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: '/app/partials/managerview.html'
            })
            
        //for accountant user    
        .when('/accountant',
            {
                controller: 'MainController',
                templateUrl: '/app/partials/accountant.html'
            })
        //view for accountant user seeing request
        .when('/accountantview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: '/app/partials/accountantview.html'
            })
            
        //for admin user    
        .when('/admin',
            {
                controller: 'MainController',
                templateUrl: '/app/partials/admin.html'
            })
        //view for admin user seeing request
        .when('/adminview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: '/app/partials/adminview.html'
            })     
        //default to standard
        .otherwise({ redirectTo: '/standard' });
});




