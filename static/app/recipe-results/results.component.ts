import { Component, OnInit } from '@angular/core';
import { Router }  from '@angular/router';
import { Recipe } from '../models/recipe';
import { DataService } from '../services/data.service';

@Component({
    selector: 'recipe-results',
    templateUrl: './app/recipe-results/results.component.html',
    providers: [DataService]
})
export class ResultsComponent implements OnInit {
    recipes: Recipe[];

    constructor(private router: Router, private dataService: DataService) { }

    getRecipes() {
        this.dataService.getRecipes().then(recipes => this.recipes = recipes);
    }

    ngOnInit() {
        this.getRecipes();
    }

    gotoDetail(recipe: Recipe) {
        console.log(recipe);
        let link = ['recipe', { id: recipe.id }];
        this.router.navigate(link);
    }
}