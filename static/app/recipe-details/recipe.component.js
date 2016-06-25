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
var data_service_1 = require('../services/data.service');
var router_1 = require('@angular/router');
var RecipeComponent = (function () {
    function RecipeComponent(route, dataService) {
        this.route = route;
        this.dataService = dataService;
    }
    RecipeComponent.prototype.ngOnInit = function () {
        // TODO: Get the id from the route
        // let id = +this.route.params.('id');
        //  this.dataService.getRecipe(id)
        //    .then(recipe => this.recipe = recipe);
    };
    RecipeComponent = __decorate([
        core_1.Component({
            selector: 'recipe-details',
            templateUrl: './app/recipe-details/recipe.component.html',
            providers: [data_service_1.DataService]
        }), 
        __metadata('design:paramtypes', [router_1.ActivatedRoute, data_service_1.DataService])
    ], RecipeComponent);
    return RecipeComponent;
}());
exports.RecipeComponent = RecipeComponent;
//# sourceMappingURL=recipe.component.js.map