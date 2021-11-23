import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface AuthResponseData{
  token:string;
  expiration:Date;
  // kind:string;
  // idToken: string;
  // email: string;
  // refreshToken: string;
  // expiresIn: string;
  // localId: string;
  // registered?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http : HttpClient) { }
  //'https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyDoZdryCEue7Cgmb7qJ9mOvsMnATXbItJc'
  signUp(email:string, password:string){
    //colocar la apikey del proyecto de firebase
    return this.http.post<AuthResponseData>(
      'https://localhost:44352/api/auth',
      {
        email: email,
        password: password,
        returnSecureToken: true
      }
    ).pipe(catchError(errorResp =>{
      let errorMessage = 'An unknown error ocurred!'
      if(!errorResp.error || !errorResp.error.error){
        return throwError(errorMessage)
      }
      switch (errorResp.error.error.message) {
        case 'EMAIL_EXISTS':
          errorMessage = 'This email exists already'
          break;
      }
      return throwError(errorMessage)
    }));
  }

signIn(email:string, password:string){
  return this.http.post<AuthResponseData>(
    'https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyDoZdryCEue7Cgmb7qJ9mOvsMnATXbItJc',
    {
      email: email,
      password: password,
      returnSecureToken: true
    }
  ).pipe(catchError(errorResp =>{
    console.log(errorResp);
    let errorMessage = 'An unknown error ocurred!'
    if(!errorResp.error || !errorResp.error.error){
      return throwError(errorMessage)
    }
    switch (errorResp.error.error.message){
      case 'EMAIL_NOT_FOUND':
        errorMessage = 'Email not found'
        break;
    }
    return throwError(errorMessage)
  }));
}

}
