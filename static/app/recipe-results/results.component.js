"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var data_service_1 = require('../services/data.service');
var ResultsComponent = (function () {
    function ResultsComponent(router, dataService) {
        this.router = router;
        this.dataService = dataService;
    }
    ResultsComponent.prototype.getRecipes = function () {
        var _this = this;
        this.dataService.getRecipes().then(function (recipes) { return _this.recipes = recipes; });
    };
    ResultsComponent.prototype.ngOnInit = function () {
        this.getRecipes();
    };
    ResultsComponent.prototype.gotoDetail = function (recipe) {
        console.log(recipe);
        var link = ['recipe', { id: recipe.id }];
        this.router.navigate(link);
    };
    ResultsComponent = __decorate([
        core_1.Component({
            selector: 'recipe-results',
            templateUrl: './app/recipe-results/results.component.html',
            providers: [data_service_1.DataService]
        }), 
        __metadata('design:paramtypes', [router_1.Router, data_service_1.DataService])
    ], ResultsComponent);
    return ResultsComponent;
}());
exports.ResultsComponent = ResultsComponent;
//# sourceMappingURL=results.component.js.map