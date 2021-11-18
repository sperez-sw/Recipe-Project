import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { RecipeService } from 'src/app/services/recipe.service';
import { Recipe } from '../recipe.model';

@Component({
  selector: 'app-recipe-detail',
  templateUrl: './recipe-detail.component.html',
  styleUrls: ['./recipe-detail.component.scss']
})
export class RecipeDetailComponent implements OnInit {
  // @Input() recipe:Recipe;
  recipe: Recipe;
  id:number;
  constructor(private recipeService:RecipeService,private router:Router ,private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params
      .subscribe(
        (params: Params) => {
          this.id = +params['id']
          this.recipe = this.recipeService.getRecipe(this.id);//Castea el id a number que por defecto es un string
        }
      );
  }

  goToSL(){
    this.recipeService.addIngredientsToSL(this.recipe.ingredients)
  }

  onEditRecipe(){
    this.router.navigate(['edit'],{relativeTo: this.route})
  }

  onDeleteRecipe(){
    this.recipeService.deleteRecipe(this.id)
    this.router.navigate(['/recipes'])
  }

}
