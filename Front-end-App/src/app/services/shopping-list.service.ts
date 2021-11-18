import { EventEmitter, Injectable } from '@angular/core';
import { Ingredient } from '../shared/ingredient.model';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShoppingListService {
  private ingredients:Ingredient[] = [new Ingredient('Apples',6),new Ingredient('Tomatoes',4)];
  ingredientChanges = new Subject<Ingredient[]>();//new EventEmitter<Ingredient[]>();
  startedEditing = new Subject<number>();
  constructor() { }

  getIngredients(){
    return this.ingredients.slice()
  }

  getIngdient(index:number){
    return this.ingredients[index];
  }

  AddIngredient(ingredient:Ingredient){
    this.ingredients.push(ingredient);
    this.ingredientChanges.next(this.ingredients.slice());
  }

  IngredientsAddedToSL(ingredients:Ingredient[]){
    this.ingredients.push(...ingredients)
    this.ingredientChanges.next(this.ingredients.slice())
  }

  updateIngredient(index:number, newIngredient: Ingredient){
    this.ingredients[index] = newIngredient;
    this.ingredientChanges.next(this.ingredients.slice())
  }

  deleteIngredient(index:number){
    this.ingredients.splice(index,1);//El 1 significa que elimina un elemento
    this.ingredientChanges.next(this.ingredients.slice());
  }
}
