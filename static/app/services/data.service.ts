import { Injectable } from '@angular/core';
import { Recipe } from '/models/recipe';
import { RECIPES } from 'app/data/mock-recipes';

@Injectable()
export class DataService {
    getRecipes() {
        return Promise.resolve(RECIPES);
    }
}