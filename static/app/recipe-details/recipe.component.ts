import { Component, OnInit } from '@angular/core';
import { Recipe } from '../models/recipe';
import { DataService } from '../services/data.service';
import { ActivatedRoute }  from '@angular/router';


@Component({
    selector: 'recipe-details',
    templateUrl: './app/recipe-details/recipe.component.html',
    providers: [DataService]
})
export class RecipeComponent implements OnInit {
    recipe: Recipe;

    constructor(private route: ActivatedRoute, private dataService: DataService) { }

    ngOnInit() {
       // TODO: Get the id from the route
       // let id = +this.route.params.('id');
      //  this.dataService.getRecipe(id)
        //    .then(recipe => this.recipe = recipe);
    }
}

