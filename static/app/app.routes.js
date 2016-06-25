"use strict";
var router_1 = require('@angular/router');
var input_component_1 = require("./voice-input/input.component");
var results_component_1 = require('./recipe-results/results.component');
var recipe_component_1 = require('./recipe-details/recipe.component');
exports.routes = [
    { path: '', component: input_component_1.InputComponent },
    { path: 'recipes', component: results_component_1.ResultsComponent },
    { path: 'recipes/:id', component: recipe_component_1.RecipeComponent }
];
exports.APP_ROUTER_PROVIDERS = [
    router_1.provideRouter(exports.routes)
];
//# sourceMappingURL=app.routes.js.map