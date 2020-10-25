using System;
using System.Dynamic;

public class RecipeCategory
{

	public string categoryName { get; set; }

	

	public RecipeCategory(string RecipeCategoryName)
    {
		categoryName = RecipeCategoryName;

	}

	public RecipeCategory()
	{
		

	}

	public string GetCategoryName()
    {
		return categoryName;
    }

	

}
