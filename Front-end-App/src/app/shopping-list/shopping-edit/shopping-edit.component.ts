import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ShoppingListService } from 'src/app/services/shopping-list.service';
import { Ingredient } from 'src/app/shared/ingredient.model';

@Component({
  selector: 'app-shopping-edit',
  templateUrl: './shopping-edit.component.html',
  styleUrls: ['./shopping-edit.component.scss']
})
export class ShoppingEditComponent implements OnInit {
  //@Output() ingredientAdded = new EventEmitter<Ingredient>();
  myForm: FormGroup;
  suscription:Subscription;
  editMode = false;
  editedItemIndex:number;
  editedItem:Ingredient;
  constructor(private shoppingListService:ShoppingListService) { }

  ngOnInit(): void {
    this.myForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'amount' : new FormControl(null, Validators.required)
    })

    this.suscription = this.shoppingListService.startedEditing
      .subscribe(
        (index:number) =>{
          this.editedItemIndex = index;
          this.editMode = true;
          this.editedItem = this.shoppingListService.getIngdient(index);
          this.myForm.setValue({
            'name': this.editedItem.name,
            'amount' : this.editedItem.amount
          })
    })
  }

  addNewIngredient(name:HTMLInputElement,amount:HTMLInputElement){
    this.shoppingListService.AddIngredient(new Ingredient(name.value,amount.valueAsNumber))
  }

  onSubmit(){
      //console.log(this.newForm.controls['email'].value);
      console.log(this.myForm);
      const newIngredient = new Ingredient(this.myForm.controls['name'].value,this.myForm.controls['amount'].value)
      if (!this.editMode){
        this.shoppingListService.AddIngredient(newIngredient)
      }
      else{
        this.shoppingListService.updateIngredient(this.editedItemIndex,newIngredient)
      }
      this.myForm.reset();
      this.editMode = false;
        //this.newForm.controls['email'].setValue("quetepasachimuelo@g.com")
  }

  onDelete(){
    this.shoppingListService.deleteIngredient(this.editedItemIndex)
    this.onClear();
  }

  onClear(){
    this.myForm.reset();
    this.editMode = false;
  }

  ngOnDestroy(){
    this.suscription.unsubscribe()
  }

}
