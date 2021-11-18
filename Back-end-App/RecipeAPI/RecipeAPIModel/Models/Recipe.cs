using System;
using System.Collections.Generic;

public class Recipe
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string imagePath { get; set; }
    public List<Ingredient> ingredients { get; set; }

}
