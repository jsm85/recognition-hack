import { Component, OnInit } from '@angular/core';
import { Recipe } from '../models/recipe';
import { DataService } from '../services/data.service';

@Component({
    selector: 'recipe-results',
    templateUrl: './app/recipe-results/results.component.html',
    providers: [DataService]
})
export class ResultsComponent implements OnInit {
    recipes: Recipe[];

    constructor(private dataService: DataService) { }

    getRecipes() {
        this.dataService.getRecipes().then(recipes => this.recipes = recipes);
    }

    ngOnInit() {
        this.getRecipes();
    }
}