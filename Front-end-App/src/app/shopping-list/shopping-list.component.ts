import { Component, EventEmitter, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ShoppingListService } from '../services/shopping-list.service';
import { Ingredient } from '../shared/ingredient.model';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.scss']
})
export class ShoppingListComponent implements OnInit, OnDestroy{
  ingredients:Ingredient[];
  private inChangeSub:Subscription;
  constructor(private shoppingListService:ShoppingListService) { }

  ngOnInit(): void {
    this.ingredients = this.shoppingListService.getIngredients()
    this.inChangeSub = this.shoppingListService.ingredientChanges.subscribe((ings:Ingredient[])=>{
      this.ingredients = ings;
    })
  }

  // newAddedIngredient(ingredient:Ingredient){
  //   this.ingredients.push(ingredient);
  // }

  ngOnDestroy(){
    this.inChangeSub.unsubscribe();
  }

  onEditItem(index:number){
    this.shoppingListService.startedEditing.next(index);
  }
}
