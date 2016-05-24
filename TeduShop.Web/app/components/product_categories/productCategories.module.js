/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop.product_categories', ['tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('product_categories', {
            url: "/product_categories",
            templateUrl: "/app/components/product_categories/productCategoryListView.html",
            parent: 'base',
            controller: "productCategoryListController"
        })
            .state('add_product_category', {
                url: "/add_product_category",
                parent: 'base',
                templateUrl: "/app/components/product_categories/productCategoryAddView.html",
                controller: "productCategoryAddController"
            })
            .state('edit_product_category', {
                url: "/edit_product_category/:id",
                templateUrl: "/app/components/product_categories/productCategoryEditView.html",
                controller: "productCategoryEditController",
                parent: 'base',
            });
    }
})();