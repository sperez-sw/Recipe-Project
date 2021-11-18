import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs/internal/Observable';
import { AuthResponseData, AuthService } from '../auth.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  isLoginMode = true;
  isLoading = false;
  error:string;
  authForm: FormGroup;
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authForm = new FormGroup({
      'email': new FormControl(null, Validators.required),
      'password' : new FormControl(null, Validators.required)
    })
  }

  onSwitchMode(){
    this.isLoginMode = !this.isLoginMode;
  }

  onSubmit(){
    //console.log(this.authForm.value)
    if(!this.authForm.valid){
      return
    }
    const password = this.authForm.value.password
    const email = this.authForm.value.email

    let authObs : Observable<AuthResponseData>;
    this.isLoading = true;
    if(this.isLoginMode){
      authObs = this.authService.signIn(email,password)
        // .subscribe(response =>{
        // console.log(response)
      // })
    }
    else{
      authObs = this.authService.signUp(email,password)
        // .subscribe(response => {
        // console.log(response)
      // },errorResp =>{
      //   this.error = errorResp;
      // });
    }
    authObs.subscribe(
      resData => {
      console.log(resData)
      this.isLoading = false;
    },
    errorMessage =>{
      console.log(errorMessage)
      this.error = errorMessage;
      this.isLoading = false;
    });
    
    this.authForm.reset();
  }
}
