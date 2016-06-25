import { provideRouter, RouterConfig }  from '@angular/router';

import { InputComponent } from "./voice-input/input.component";
import { ResultsComponent } from './recipe-results/results.component';
import { RecipeComponent } from './recipe-details/recipe.component';

export const routes: RouterConfig = [
    { path: '', component: InputComponent },
    { path: 'recipes', component: ResultsComponent },
    { path: 'recipes/:id', component: RecipeComponent }
];

export const APP_ROUTER_PROVIDERS = [
    provideRouter(routes)
];