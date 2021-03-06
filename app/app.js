
var app = angular.module('requestsApp', ['ui.event', 'ui.autocomplete', 'ngRoute','angularUtils.directives.dirPagination', 'ui.bootstrap']);

//This configures the routes and associates each route with a view and a controller
app.config(function ($routeProvider) {
    $routeProvider
    
        //for standard user
        .when('/standard',
            {
                controller: 'MainController',
                templateUrl: 'app/partials/standard.html'
            })
        //view for standard user seeing request
        .when('/standardview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: 'app/partials/standardview.html'
            })
            
        //for manager user
        .when('/manager',
            {
                controller: 'MainController',
                templateUrl: 'app/partials/manager.html'
            })
        //view for manager user seeing request
        .when('/managerview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: 'app/partials/managerview.html'
            })
            
        //for accountant user    
        .when('/accountant',
            {
                controller: 'MainController',
                templateUrl: 'app/partials/accountant.html'
            })
        //view for accountant user seeing request
        .when('/accountantview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: 'app/partials/accountantview.html'
            })
            
        //for admin user    
        .when('/admin',
            {
                controller: 'MainController',
                templateUrl: 'app/partials/admin.html'
            })
        //view for admin user seeing request
        .when('/adminview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: 'app/partials/adminview.html'
            })     
        .when('/adminsuppliers',
            {
                controller: 'SupplierController',
                templateUrl: 'app/partials/adminsuppliers.html'
            }) 
        .when('/adminsuppliersview/:supID',
            {
                controller: 'SupplierViewController',
                templateUrl: 'app/partials/adminsuppliersview.html'
            })             
        .when('/admincodes',
            {
                controller: 'CodeController',
                templateUrl: 'app/partials/adminaccountantcodes.html'
            }) 
        .when('/admincodesview/:codeID',
            {
                controller: 'CodeViewController',
                templateUrl: 'app/partials/adminaccountantcodesview.html'
            })  
        .when('/accountantcodes',
            {
                controller: 'CodeController',
                templateUrl: 'app/partials/adminaccountantcodes.html'
            }) 
        .when('/accountantcodesview/:codeID',
            {
                controller: 'CodeViewController',
                templateUrl: 'app/partials/adminaccountantcodesview.html'
            })
        //for system admin user    
        .when('/systemadmin',
            {
                controller: 'MainController',
                templateUrl: 'app/partials/systemadmin.html'
            })
        //view for system admin user seeing request
        .when('/systemadminview/:reqID',
            {
                controller: 'MainViewController',
                templateUrl: 'app/partials/systemadminview.html'
            })      
        .when('/systemadminusers',
            {
                controller: 'UserController',
                templateUrl: 'app/partials/systemadminusers.html'
            })             
            
        //default to standard
        .otherwise({ redirectTo: '/standard' });
});


