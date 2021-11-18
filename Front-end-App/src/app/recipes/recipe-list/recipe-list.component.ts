import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RecipeService } from 'src/app/services/recipe.service';
import { Recipe } from '../recipe.model';

@Component({
  selector: 'app-recipe-list',
  templateUrl: './recipe-list.component.html',
  styleUrls: ['./recipe-list.component.scss']
})
export class RecipeListComponent implements OnInit {
  recipes:Recipe[];
  @Output() recipeWasSelected = new EventEmitter<Recipe>();
  constructor(private recipeService: RecipeService, private router:Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.recipes = this.recipeService.getRecipes()
    this.recipeService.recipesChanged.subscribe((re)=>{
      this.recipes = re;
    })
  }

  onRecipeSelected(recipeEl:Recipe){
    this.recipeWasSelected.emit(recipeEl);
  }

  addNewRecipe(){
    this.router.navigate(['new'],{relativeTo: this.route})
  }

}
