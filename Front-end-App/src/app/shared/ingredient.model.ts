export class Ingredient{
    public id?:number;
    public name:string;
    public amount:number
    constructor(name:string,amount:number){
        this.name = name;
        this.amount = amount;
    }
}